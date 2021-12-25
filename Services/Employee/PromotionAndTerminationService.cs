using System;
using System.Diagnostics.CodeAnalysis;
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

namespace CDFStaffManagement.Services.Employee
{
    public class PromotionAndTerminationService : IPromotionAndTerminationService
    {
        private readonly MyPayrollContext _dbContext;
        private readonly IEmployeeObjectRepository _employeeObjectRepository;
        private readonly IPositionCodeDetailsService _positionCodeDetails;
        private readonly ICustomLogger _customLogger;


        public PromotionAndTerminationService(MyPayrollContext dbContext, IEmployeeObjectRepository employeeObjectRepository, ICustomLogger customLogger, IPositionCodeDetailsService positionCodeDetails)
        {
            _dbContext = dbContext;
            _employeeObjectRepository = employeeObjectRepository;
            _customLogger = customLogger;
            _positionCodeDetails = positionCodeDetails;
        }
 
        /**
         * This method is used to terminate an employee
         */
        public async Task<ResponseModel> TerminateEmployee(EmployeeTerminationDto terminationRequest)
        {
            if (terminationRequest == null)
            {
                return ResponseEntity.GetResponse(ResponseConstants.RequiredDataNotProvided, 500, false);
            }

            var employee = await _employeeObjectRepository.GetActiveEmployee(terminationRequest.EmployeeCode!);

            if (employee == null)
            {
                return ResponseEntity.GetResponse(ResponseConstants.EmployeeNotFound, 500, false);
            }

            var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                await UpdateEntity(employee.EntityCode, terminationRequest, ActionCodes.Terminate.ToString());
                await transaction.CommitAsync();
                return ResponseEntity.GetResponse(ResponseConstants.EmployeeTerminatedSuccessfully, 200, true);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }

        /**
         * This method is used to reinstate an employee
         */
        public async Task<ResponseModel> ReinstateEmployee(string employeeCode)
        {
            if (string.IsNullOrEmpty(employeeCode))
            {
                return ResponseEntity.GetResponse(ResponseConstants.RequiredDataNotProvided, 500, false);
            }

            var terminatedEmployee = await _employeeObjectRepository.GetTerminatedEmployee(employeeCode);

            if (terminatedEmployee == null)
            {
                return ResponseEntity.GetResponse(ResponseConstants.EmployeeNotFound, 500, false);
            }

            var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var terminateRequest = new EmployeeTerminationDto
                {
                    EmployeeCode = employeeCode
                };
                await UpdateEntity(terminatedEmployee.EntityCode, terminateRequest, ActionCodes.Reinstate.ToString());
                await transaction.CommitAsync();
                return ResponseEntity.GetResponse(ResponseConstants.EmployeeReinstatedSuccessfully, 200, true);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }
        
        /**
         * This method is used to effect employee promotion/transfer
         */
        public async Task<ResponseModel> PromoteEmployee(PromotionAndTransferDto promotionRequest)
        {
            var employee = await _dbContext.Employee
                .Where(x => x.EmployeeCode == promotionRequest.EmployeeCode && x.EmployeeStatusId != 5)
                .FirstOrDefaultAsync();

            if (employee == null)
            {
                return ResponseEntity.GetResponse(ResponseConstants.EmployeeNotFound, 500, false);
            }

            var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                await UnlinkEmployeeFromPositionCode(promotionRequest.EmployeeCode!);

                var linkEmployeeToNewPosition =
                    await _positionCodeDetails.LinkEmployeeToPositionCode(
                        promotionRequest.EmployeeCode!,
                        promotionRequest.NewPositionCode!);

                var isEmployeeLinked = linkEmployeeToNewPosition.Success?? false;
                if (!isEmployeeLinked)
                {
                    await transaction.RollbackAsync();
                    return ResponseEntity.GetResponse(linkEmployeeToNewPosition.Message!, 500, false);
                }

                await CreateEmployeeHistoryLine(employee, null, promotionRequest, true);
                await transaction.CommitAsync();
                return ResponseEntity.GetResponse(ResponseConstants.EmployeePromotedSuccessfully, 200, true);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }

        private async Task UpdateEntity(int entityCode,  EmployeeTerminationDto terminationRequest, string action)
        {
            var entity = await _dbContext.Entity
                .Where(x => x.EntityCode == entityCode)
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                throw new Exception("entity not found");
            }

            var statusId = action.Equals(ActionCodes.Terminate.ToString()) ? 5 : 1;
            entity.EmployeeStatusId = statusId;
            await _dbContext.SaveChangesAsync();
            await UpdateEmployee(terminationRequest, action);
        }

        private async Task UpdateEmployee( EmployeeTerminationDto terminationDto, string action)
        {
            var employee = await _dbContext.Employee
                .Where(x => x.EmployeeCode == terminationDto.EmployeeCode)
                .FirstOrDefaultAsync();

            if (employee == null)
            {
                throw new Exception("employee not found");
            }

            var statusId = 5;
            DateTime? terminationDate = terminationDto.TerminationDate;
            int? terminationReason = terminationDto.TerminationReason;

            if (action.Equals(ActionCodes.Reinstate.ToString()))
            {
                statusId = 1;
                terminationDate = null;
                terminationReason = null;
                CreateAuditLog(statusId.ToString(), "5", "EmployeeStatusId", employee.EmployeeId);
            }
            else
            {
                await CreateEmployeeHistoryLine(employee, terminationDto, null,false);
                await UnlinkEmployeeFromPositionCode(terminationDto.EmployeeCode!);
            }

            var username = _customLogger.GetCurrentUser();
            var userId = _customLogger.GetUserByUserName(username)?.Id;
            employee.TerminationDate = terminationDate;
            employee.TerminationReasonId = terminationReason;
            employee.EmployeeStatusId = statusId;
            employee.ModifiedBy = userId;
            employee.ModifiedDate = DateTime.Now;
            await _dbContext.SaveChangesAsync();
        }
        
        private async Task UnlinkEmployeeFromPositionCode(string employeeCode)
        {
            var position = await _dbContext.PositionDetails
                .Where(x => x.EmployeeCode == employeeCode)
                .FirstOrDefaultAsync();

            if (position != null)
            {
                position.EmployeeCode = null;
                position.VacancyDate = DateTime.Now;
                position.Status = ResponseConstants.Vacant;
                await _dbContext.SaveChangesAsync();
            }
        }
        
        private async Task CreateEmployeeHistoryLine(Model.EntityModels.Employee employee, EmployeeTerminationDto terminationDto, PromotionAndTransferDto promotionAndTransferDto, bool isPromotionRequest)
        {
            string positionCode;
            int? terminationReason = null;
            DateTime? terminationDate = null;
            
            //create audit log
            if (isPromotionRequest)
            {
                positionCode = promotionAndTransferDto.OldPositionCode!;
                await UpdateEmployeeJobGrade(promotionAndTransferDto.NewPositionCode!, employee);
                await LogPromotionHistory(promotionAndTransferDto);
                CreateAuditLog(
                    promotionAndTransferDto.NewPositionCode!,
                    promotionAndTransferDto.OldPositionCode!, 
                    "PositionCode", 
                    employee.EmployeeId
                    ); 
            }
            else
            {
                positionCode = terminationDto.PositionCode!.Trim();
                terminationDate = terminationDto.TerminationDate;
                terminationReason = terminationDto.TerminationReason;
                
                CreateAuditLog(terminationDto.TerminationDate.ToString(CultureInfo.InvariantCulture),
                    employee.TerminationDate.ToString()!, "TerminationDate", employee.EmployeeId);
                
                CreateAuditLog(terminationDto.TerminationReason.ToString(CultureInfo.InvariantCulture),
                    employee.TerminationReasonId.ToString()!, "TerminationReason", employee.EmployeeId);
            }
            
            var employeeHistory = new EmployeeHistory
            {
                EmployeeId = employee.EmployeeId,
                EmployeeCode = employee.EmployeeCode,
                EmployeeStatusId = 5,
                TerminationDate = terminationDate,
                TerminationReasonId = terminationReason,
                DateEngaged = employee.DateEngaged,
                JobGradeId = employee.JobGradeId,
                JobTitleId = employee.JobTitleId,
                PositionCode = positionCode,
                JobGeneralId = employee.JobGeneralId,
                NatureOfContractId = employee.NatureOfContractId,
                CreatedBy = _customLogger.GetCurrentUser(),
                CreatedDate = DateTime.Now
            };
            await _dbContext.AddAsync(employeeHistory);
            await _dbContext.SaveChangesAsync();
        }
        private void CreateAuditLog(string newValue, string oldValue, string fieldName, int empId)
        {
            var auditLogs = new UserAuditLogs()
            {
                EmployeeId = empId,
                Action = fieldName + " amendment",
                FieldName = fieldName,
                ActionType = "Update",
                OldValue = oldValue,
                NewValue = newValue
            };
            _customLogger.CreateAuditLog(auditLogs);
        }

        private async Task LogPromotionHistory(PromotionAndTransferDto promotionAndTransferDto)
        {
            var userId = await _customLogger.GetUserId();

            var promotionHistory = new PromotionHistory
            {
                EmployeeCode = promotionAndTransferDto.EmployeeCode,
                OldPositionCode = promotionAndTransferDto.OldPositionCode,
                NewPositionCode = promotionAndTransferDto.NewPositionCode,
                PromotionDate = promotionAndTransferDto.StartDate,
                CreatedDate = DateTime.Now,
                CreatedBy = userId
            };
            await _dbContext.PromotionHistory.AddAsync(promotionHistory);
            await _dbContext.SaveChangesAsync();
        }

        private async Task UpdateEmployeeJobGrade(string positionCode, Model.EntityModels.Employee employee)
        {
            var jobCode = positionCode[..4]?.ToUpper().Trim();
            var jobTitle = await _dbContext.JobTitle.Where(x => x.Jobcode!.Equals(jobCode)).FirstOrDefaultAsync();

            if (jobTitle == null)
            {
                throw new Exception("Failed to retrieve job title");
            }
            employee.JobTitleId = jobTitle.JobTitleId;
            employee.JobGradeId = jobTitle.JobGradeId;
            employee.ModifiedBy = await _customLogger.GetUserId();
            employee.ModifiedDate = DateTime.Now;
            await _dbContext.SaveChangesAsync();
        }
        
    }
    public enum ActionCodes
    {
        Terminate,
        Reinstate
    }
}

