using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CDFStaffManagement.Enums;
using CDFStaffManagement.Interfaces;
using CDFStaffManagement.Model.DBContext;
using CDFStaffManagement.Services.Employee.Dto;
using CDFStaffManagement.Utilities;
using Microsoft.EntityFrameworkCore;
using MyPayroll;


namespace CDFStaffManagement.Services.Employee
{
    public class BanksDetailsService : IBanksDetailsService
    {
        private readonly MyPayrollContext _dbContext;
        private readonly IEmployeeObjectRepository _employeeObjectRepository;
        private readonly ICustomLogger _customLogger;

        public BanksDetailsService(MyPayrollContext dbContext, IEmployeeObjectRepository employeeObjectRepository,
            ICustomLogger customLogger)
        {
            _dbContext = dbContext;
            _employeeObjectRepository = employeeObjectRepository;
            _customLogger = customLogger;
        }

        /**
         * This method is used to retrieve bank details by employee code
         */
        public async Task<ResponseModel> GetBankDetailsByEmployeeCode(string employeeCode)
        {
            if (string.IsNullOrEmpty(employeeCode))
            {
                throw new Exception(ResponseConstants.EmployeeCodeNotFound);
            }

            var employee = await _employeeObjectRepository.GetActiveEmployee(employeeCode);

            if (employee == null)
            {
                return ResponseEntity.GetResponse(ResponseConstants.EmployeeNotFound, 500, false);
            }

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EmployeeBankDetailsView, BankDetailsViewDto>();
            });
            var iMapper = config.CreateMapper();
            var bankDetails = await _dbContext.EmployeeBankDetailsView
                .OrderByDescending(x => x.DateCreated)
                .Where(x => x.EmployeeCode == employeeCode)
                .ToListAsync();

            var mappedList = bankDetails.Select(item => iMapper.Map<BankDetailsViewDto>(item)).ToList();

            return ResponseEntity.GetResponse(ResponseConstants.Success, 200, true, mappedList);
        }

        /**
         * This method is used to update bank details for a given employee
         */
        public async Task<ResponseModel> UpdateBankDetails(BankDetailsViewDto bankDetailsViewDto, int bankId,
            int branchId)
        {
            if (bankDetailsViewDto == null)
            {
                throw new Exception(ResponseConstants.RequiredDataNotProvided);
            }

            var employee = await _employeeObjectRepository.GetActiveEmployee(bankDetailsViewDto.EmployeeCode!);

            if (employee == null)
            {
                return ResponseEntity.GetResponse(ResponseConstants.EmployeeNotFound, 500, false);
            }

            var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var bankDetails = new EmployeeBankDetails
                {
                    EmployeeId = employee.EmployeeId,
                    BankId = bankId,
                    BranchId = branchId,
                    AccountName = bankDetailsViewDto.AccountName,
                    AccountNumber = bankDetailsViewDto.AccountNumber,
                    CreatedBy = _customLogger.GetCurrentUser(),
                    StatusId = 1,
                    IsDefaultBank = 1,
                    DateCreated = DateTime.Now
                };

                await SaveEmployeeBankDetails(bankDetails);
                await transaction.CommitAsync();
                return ResponseEntity.GetResponse(ResponseConstants.TransactionSuccessful, 200, true);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }

        /**
         * This method is used to a bank for an employee
         */
        public async Task<ResponseModel> SaveEmployeeBankDetails(EmployeeBankDetails bankDetails)
        {
            if (bankDetails == null)
            {
                throw new Exception(ResponseConstants.RequiredDataNotProvided);
            }
            await DisableCurrentBankDetails(bankDetails.EmployeeId);
            await _dbContext.AddAsync(bankDetails);
            await _dbContext.SaveChangesAsync();
            return ResponseEntity.GetResponse(ResponseConstants.Success, 200, true);
        }

        /**
         * This method is used to change the default bank for an employee
         */
        public async Task<ResponseModel> ChangeDefaultBank(string employeeCode, string accountNumber)
        {
            if (string.IsNullOrEmpty(employeeCode) || string.IsNullOrEmpty(accountNumber))
            {
                throw new Exception(ResponseConstants.RequiredDataNotProvided);
            }

            var employee = await _employeeObjectRepository.GetActiveEmployee(employeeCode);

            if (employee == null)
            {
                return ResponseEntity.GetResponse(ResponseConstants.EmployeeNotFound, 500, false);
            }

            var bankDetail = await _dbContext.EmployeeBankDetails
                .Where(x => x.EmployeeId == employee.EmployeeId && x.AccountNumber == accountNumber)
                .FirstOrDefaultAsync();

            if (bankDetail == null)
            {
                return ResponseEntity.GetResponse("Error occured while retrieving the bank account details", 500,
                    false);
            }

            var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                await DisableCurrentBankDetails(employee.EmployeeId);
                bankDetail.StatusId = 1;
                bankDetail.IsDefaultBank = 1;
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return ResponseEntity.GetResponse(ResponseConstants.TransactionSuccessful, 200, true);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }

        private async Task DisableCurrentBankDetails(int employeeId)
        {
            if (employeeId == 0)
            {
                return;
            }

            var activeBankDetails = await _dbContext.EmployeeBankDetails
                .Where(x => x.EmployeeId == employeeId).ToListAsync();

            if (activeBankDetails.Any())
            {
                foreach (var item in activeBankDetails)
                {
                    item.StatusId = 2;
                    item.IsDefaultBank = 0;
                    await _dbContext.SaveChangesAsync();
                }
            }
        }
    }
}