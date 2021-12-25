using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CDFStaffManagement.Enums;
using CDFStaffManagement.Interfaces;
using CDFStaffManagement.Model.DBContext;
using CDFStaffManagement.Model.EntityModels;
using CDFStaffManagement.Services.Employee.Dto;
using CDFStaffManagement.Utilities;
using Microsoft.EntityFrameworkCore;

namespace CDFStaffManagement.Services.Employee
{
    public class EmployeeRemunerationService : IEmployeeRemunerationService
    {
        private readonly MyPayrollContext _dbContext;
        private readonly IEmployeeObjectRepository _employeeObjectRepository;
        private readonly ICustomLogger _customLogger;

        public EmployeeRemunerationService(MyPayrollContext dbContext,
            IEmployeeObjectRepository employeeObjectRepository, ICustomLogger customLogger)
        {
            _dbContext = dbContext;
            _employeeObjectRepository = employeeObjectRepository;
            _customLogger = customLogger;
        }

        /**
         * get remuneration history for a given employee
         */
        public async Task<ResponseModel> GetEmployeeRemunerationHistory(string employeeCode)
        {
            if (string.IsNullOrEmpty(employeeCode))
            {
                return ResponseEntity.GetResponse(ResponseConstants.RequiredDataNotProvided, 500, false);
            }

            var employee = await _employeeObjectRepository.GetActiveEmployee(employeeCode);

            if (employee == null)
            {
                return ResponseEntity.GetResponse(ResponseConstants.EmployeeNotFound, 500, false);
            }

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EmployeeRemuneration, EmployeeRemunerationDto>();
            });
            var iMapper = config.CreateMapper();
            var configurations = await _dbContext.EmployeeRemuneration
                .OrderByDescending(x=>x.DateCreated)
                .Where(x => x.EmployeeId == employee.EmployeeId)
                .ToListAsync();

            var mappedList = configurations.Select(item => iMapper.Map<EmployeeRemunerationDto>(item)).ToList();

            return ResponseEntity.GetResponse(ResponseConstants.Success, 200, true, mappedList);
        }

        /**
         * Update the employee's basic pay
         */
        public async Task<ResponseModel> UpdateEmployeeRemuneration(EmployeeRemunerationDto remunerationDto)
        {
            if (remunerationDto == null)
            {
                return ResponseEntity.GetResponse(ResponseConstants.RequiredDataNotProvided, 500, false);
            }

            var employee = await _employeeObjectRepository.GetActiveEmployee(remunerationDto.EmployeeCode!);

            if (employee == null)
            {
                return ResponseEntity.GetResponse(ResponseConstants.EmployeeNotFound, 500, false);
            }

            //expire current value(s)
            await ExpireCurrentRemunerationLine(employee.EmployeeId, remunerationDto.StartDate);
            //create new value
            return await CreateNewRemunerationLine(remunerationDto, employee.EmployeeId);
        }

        /**
         * Add employee's basic pay
         */
        public async Task<ResponseModel> CreateRemunerationLine(EmployeeRemunerationDto request, int employeeId)
        {
            if (request == null || employeeId == 0)
            {
                return ResponseEntity.GetResponse(ResponseConstants.RequiredDataNotProvided, 500, false);
            }

            return await CreateNewRemunerationLine(request, employeeId);
        }
        
        private async Task<ResponseModel> CreateNewRemunerationLine(EmployeeRemunerationDto request, int employeeId)
        {
            await _dbContext.EmployeeRemuneration.AddAsync(new EmployeeRemuneration
            {
                EmployeeId = employeeId,
                RemunerationAmount = request.RemunerationAmount,
                StartDate = request.StartDate,
                Reason = request.Reason,
                DateCreated = DateTime.Now,
                UserId = _customLogger.GetCurrentUser()
            });
            await _dbContext.SaveChangesAsync();

            return ResponseEntity.GetResponse(ResponseConstants.RecordAddedSuccessfully, 200, true);
        }
        private async Task ExpireCurrentRemunerationLine(int employeeId, DateTime startDate)
        {
            var currentActiveLinesAsync =
                await _dbContext.EmployeeRemuneration.Where(x => x.EmployeeId == employeeId && x.EndDate == null)
                    .ToListAsync();

            if (currentActiveLinesAsync.Any())
            {
                var transaction = await _dbContext.Database.BeginTransactionAsync();
                foreach (var item in currentActiveLinesAsync)
                {
                    item.EndDate = GetEndDate(startDate);
                    item.DateModified = DateTime.Now;
                    item.UserId = _customLogger.GetCurrentUser();
                    await _dbContext.SaveChangesAsync();
                }
                await transaction.CommitAsync();
            }
        }
        private static DateTime GetEndDate(DateTime startDate)
        {
            var today = DateTime.Now;
            var endDate = today;

            if (startDate > today)
            {
                endDate = startDate;
            }
            return endDate;
        }
    }
}