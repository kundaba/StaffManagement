using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CDFStaffManagement.Enums;
using CDFStaffManagement.Interfaces;
using CDFStaffManagement.Model.DBContext;
using CDFStaffManagement.Model.EntityModels;
using CDFStaffManagement.Services.Employee.Dto;
using CDFStaffManagement.Utilities;
using Microsoft.EntityFrameworkCore;
using MyPayroll;

namespace CDFStaffManagement.Services.Employee
{
    public class EmployeeService : IEmployeeService
    {
        private readonly MyPayrollContext _dbContext;
        private readonly ICustomLogger _customLogger;
        private readonly IEmployeeRemunerationService _employeeRemunerationService;
        private readonly IBanksDetailsService _banksDetailsService;

        public EmployeeService(MyPayrollContext payrollContext, ICustomLogger customLogger, IEmployeeRemunerationService employeeRemunerationService, IBanksDetailsService banksDetailsService)
        {
            _dbContext = payrollContext;
            _customLogger = customLogger;
            _employeeRemunerationService = employeeRemunerationService;
            _banksDetailsService = banksDetailsService;
        }

        /**
         * This method is used for auto complete searching
         * It takes in search string as the parameter
         */
        public async Task<List<string>> EmployeeAutoCompleteSearch(string searchTerm)
        {
            Debug.Assert(_dbContext.EmployeeDetail != null, "_dbContext.EmployeeDetail != null");
            var employeeList = await _dbContext.EmployeeDetail.ToListAsync();

            var empList = (
                from item in employeeList
                where item.StatusId != 5
                select new Dto.Employee
                {
                    EmployeeCode = item.EmployeeCode,
                    FullName = item.FirstName + " " + item.LastName+" ("+item.EmployeeCode + ")"
                }).ToList();

            var data = empList.Where(x => x.FullName!.ToLower().Contains(searchTerm.ToLower()))
                .Select(y => y.FullName!.ToLower()).Take(5).ToList();
            return data;
        }

        public async Task<List<EmployeeDetail>> EmployeeList()
        {
            var employeeList = await _dbContext.EmployeeDetail.OrderByDescending(x=>x.EmployeeId).ToListAsync();
            return employeeList ?? new List<EmployeeDetail>();
        }

        /**
         * This method used to to amend employee details eg firstname, lastname, nationality etc
         */
        public async Task<ResponseModel> AmendEmployeeDetails(EmployeeAmendmentDto amendmentDto)
        {
            if (amendmentDto == null)
            {
                throw new Exception(ResponseConstants.RequiredDataNotProvided);
            }

            var employee = await _dbContext.Employee
                .Where(x => x.EmployeeCode == amendmentDto.EmployeeCode && x.EmployeeId != 5)
                .FirstOrDefaultAsync();

            if (employee == null)
            {
                return ResponseEntity.GetResponse(ResponseConstants.EmployeeNotFound, 500, false);
            }

            var entity = await _dbContext.Entity.Where(x => x.EntityCode == employee.EntityId).FirstOrDefaultAsync();

            var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                for (var i = 0; i < amendmentDto.FieldName!.Length; i++)
                {
                    var fieldName = amendmentDto.FieldName[i];
                    var currentValue = amendmentDto.CurrentValue![i];
                    var newValue = amendmentDto.NewValue![i];

                    SaveAmendedValues(fieldName, entity, newValue, employee);
                    await _dbContext.SaveChangesAsync();

                    CreateAuditLog(newValue, currentValue, fieldName, employee.EmployeeId);
                }

                await transaction.CommitAsync();            
                return ResponseEntity.GetResponse(ResponseConstants.FieldsAmendedSuccessfully, 200, true);
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await transaction.RollbackAsync();
                return ResponseEntity.GetResponse(e.Message, 500, false);
            }
            
        }
        
        /**
         * Gets a list of employees by a given status
         */
        public async Task<List<EmployeeDetail>> GetEmployeesByStatus(IEnumerable<int> statusId)
        {
            var employeeDetails = new List<EmployeeDetail>();
            foreach (var id in statusId)
            {
                var employeeList = await _dbContext.EmployeeDetail.Where(x => x.StatusId == id).ToListAsync();

                if (employeeList.Any())
                {
                    employeeDetails.AddRange(employeeList);
                }
            }
            
            return employeeDetails;
        }

        /**
         * Gets employee object by employee code
         */
        public async Task<EmployeeDetail> GetEmployeeByEmployeeCode(string employeeCode)
        {
            return await _dbContext.EmployeeDetail
                .Where(x => x.EmployeeCode == employeeCode)
                .FirstOrDefaultAsync();
        }

        /**
         * Checks if the employee code already exists in the system
         */
        public async Task<bool> EmployeeExistenceCheck(string employeeCode)
        {
            return await _dbContext.Employee
                .Where(x => x.EmployeeCode == employeeCode).AnyAsync();
        }

        /**
         * This method is used to add new employee
         * it expects the new employee object as the parameter
         * Returns a ResponseModel
         */
        public async Task<ResponseModel> AddNewEmployeeAsync(NewEmployee newEmployee)
        {
            if (string.IsNullOrEmpty(newEmployee.EmployeeCode) ||
                await EmployeeExistenceCheck(newEmployee.EmployeeCode))
            {
                return ResponseEntity.GetResponse("EmployeeCode can not be null and must not already exist", 500,
                    false);
            }

            var transaction = await _dbContext.Database.BeginTransactionAsync();

            var user = _customLogger.GetCurrentUser();
            try
            {
                var newEntityCode = await SaveEntityDetailsAsync(newEmployee, user);

                if (newEntityCode == 0)
                {
                    return ResponseEntity.GetResponse(ResponseConstants.SomethingWentWrong, 500, false);
                }

                var response = await SaveEmployeeDetailAsync(newEmployee, newEntityCode, user) == 1
                    ? ResponseConstants.EmployeeAddedSuccessfully
                    : ResponseConstants.SomethingWentWrong;

                await transaction.CommitAsync();
                return ResponseEntity.GetResponse(response, 200, true);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ResponseEntity.GetResponse(ex.Message, 500, false);
            }
        }

        private async Task<int> SaveEntityDetailsAsync(NewEmployee newEmployee, string? user)
        {
            var newEntity = new Entity
            {
                DisplayName = newEmployee.FirstName![0] + " " + newEmployee!.LastName,
                FirstName = newEmployee.FirstName?.Trim().ToUpperInvariant(),
                LastName = newEmployee.LastName?.Trim().ToUpperInvariant(),
                SecondName = newEmployee.SecondName?.Trim().ToUpperInvariant(),
                MaidenName = newEmployee.MaidenName?.Trim().ToUpperInvariant(),
                BirthDate = newEmployee.BirthDate,
                Gender = newEmployee.Gender?.Trim(),
                Nationality = newEmployee.Nationality.ToString().Trim(),
                MaritalStatusId = newEmployee.MaritalStatusId,
                Idnumber = newEmployee.IdNumber?.Trim(),
                IdnumberType = newEmployee.IdNumberType,
                EmployeeStatusId = 1,
                SocialSecurityNumber = newEmployee.SocialSecurityNumber,
                WorkNumber = newEmployee.WorkNumber,
                CellNumber = newEmployee.CellNumber,
                EmailAddress = newEmployee.EmailAddress,
                PhysicalAddress = newEmployee.PhysicalAddress?.Trim().ToUpperInvariant(),
                CountryOfBirthId = newEmployee.Nationality,
                AccountName = newEmployee.AccountName!.Trim(),
                CreatedBy = user,
                DateCreated = DateTime.Now,
                LastChanged = DateTime.Now
            };
            await _dbContext.AddAsync(newEntity);
            await _dbContext.SaveChangesAsync();

            var isTaskCompletedSuccessfully = _dbContext.SaveChangesAsync().IsCompletedSuccessfully;
            return isTaskCompletedSuccessfully
                ? newEntity.EntityCode : 0;
        }
        
        private async Task<int> SaveEmployeeDetailAsync(NewEmployee employee, int entityCode, string? user)
        {
            var newEmployee = new Model.EntityModels.Employee
            {
                EntityId = entityCode,
                EmployeeCode = employee.EmployeeCode?.ToUpper().Trim(),
                DateEngaged = employee.DateEngaged,
                LeaveStartDate = employee.DateEngaged,
                PensionStartDate = employee.DateEngaged,
                TerminationDate = employee.NatureOfContractId == 1 ? employee.TerminationDate : null,
                TerminationReasonId = employee.TerminationReasonId,
                JobTitleId = employee.JobTitleId,
                JobGeneralId = employee.JobGeneralId,
                JobGradeId = employee.JobGradeId,
                EmployeeStatusId = 1,
                NatureOfContractId = employee.NatureOfContractId,
                DepartmentId = employee.DepartmentId,
                DateCreated = DateTime.Now,
                CreatedBy = _customLogger.GetUserByUserName(user)?.Id
            };
            await _dbContext.Employee.AddAsync(newEmployee);
            await _dbContext.SaveChangesAsync();
            var empId = newEmployee.EmployeeId;

            if (employee.BankId != null && employee.BankBranchId != null)
            {
                var bankDetails = new EmployeeBankDetails
                {
                    EmployeeId = empId,
                    BankId = (int)employee.BankId,
                    BranchId = (int)employee.BankBranchId,
                    AccountName = employee.AccountName,
                    AccountNumber = employee.AccountNumber,
                    CreatedBy = _customLogger.GetCurrentUser(),
                    StatusId = 1,
                    IsDefaultBank = 1,
                    DateCreated = DateTime.Now
                };

                try
                {
                    //invoke the interface for saving the bank details
                    await _banksDetailsService.SaveEmployeeBankDetails(bankDetails);
                }
                catch (Exception ex)
                {
                    _customLogger.ErrorLog(ex.Message);
                }
            }
            
            var isBasicSaved = await SaveBasicPayAsync(employee.BasicPay, empId, user);

            return isBasicSaved ? 1 : 0;
        }
        private async Task<bool> SaveBasicPayAsync(decimal amount, int employeeId, string? user)
        {
            if (string.IsNullOrEmpty(employeeId.ToString()) ||
                string.IsNullOrEmpty(amount.ToString(CultureInfo.InvariantCulture)))
            {
                return false;
            }

            var employeeRemuneration = new EmployeeRemunerationDto
            {
                RemunerationAmount = amount,
                StartDate = DateTime.Now,
                Reason = ResponseConstants.NewEmployee,
                DateCreated = DateTime.Now,
                UserId = user
            };
            
            var res = await _employeeRemunerationService.CreateRemunerationLine(employeeRemuneration, employeeId);
            return res.Success?? false;
        }

        private static void SaveAmendedValues(string fieldName, Entity entity, string newValue, Model.EntityModels.Employee emp)
        {
            switch (fieldName)
            {
                case "FirstName":
                    entity.FirstName = newValue.ToUpper().Trim();
                    break;

                case "SecondName":
                    entity.SecondName = newValue.ToUpper().Trim();
                    break;

                case "LastName":
                    entity.LastName = newValue.ToUpper().Trim();
                    break;

                case "MobileNumber":
                    entity.CellNumber = newValue.Trim();
                    break;

                case "EmailAddress":
                    entity.EmailAddress = newValue.Trim();
                    break;

                case "PhysicalAddress":
                    entity.PhysicalAddress = newValue.ToUpper().Trim();
                    break;

                case "Nationality":
                    entity.Nationality = newValue.ToUpper().Trim();
                    break;

                case "IDNumber":
                    entity.Idnumber = newValue.Trim();
                    break;

                case "SocialSecurityNumber":
                    entity.SocialSecurityNumber = int.Parse(newValue.Trim());
                    break;

                case "TerminationDate":
                    var terminationDate = DateTime.Parse(newValue.Trim());
                    if (terminationDate < DateTime.Now)
                    {
                        emp.EmployeeStatusId = 5;
                    }
                    emp.TerminationDate = terminationDate;
                    break;

                case "MaritalStatus":
                    entity.MaritalStatusId = 1;
                    break;
            }
        }
        private void CreateAuditLog(string newValue, string currentValue, string fieldName, int empId)
        {
            var auditLogs = new UserAuditLogs
            {
                EmployeeId = empId,
                Action = fieldName + " amendment",
                FieldName = fieldName,
                ActionType = "Update",
                OldValue = currentValue,
                NewValue = newValue
            };
            _customLogger.CreateAuditLog(auditLogs);
        }
    }
}