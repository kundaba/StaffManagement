using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CDFStaffManagement.Enums;
using CDFStaffManagement.Interfaces;
using CDFStaffManagement.Model.DBContext;
using CDFStaffManagement.Model.EntityModels;
using CDFStaffManagement.Services.Parameters;
using CDFStaffManagement.Utilities;

namespace CDFStaffManagement.Services.PayrollDefinitions
{
    public class PayrollDefinitionsRepository : IPayslipDefinitionRepository
    {
        private readonly MyPayrollContext _dbContext;
        private readonly ICustomLogger _customLogger;
        private readonly IEmployeeObjectRepository _employeeObjectRepository;

        public PayrollDefinitionsRepository(MyPayrollContext context, ICustomLogger customLogger,
            IEmployeeObjectRepository employeeObjectRepository)
        {
            _dbContext = context;
            _customLogger = customLogger;
            _employeeObjectRepository = employeeObjectRepository;
        }

        public async Task<List<PayrollEarningLine>> GetPayrollEarningLines()
        {
            var earningLines = await _dbContext.PayrollEarningDef
                .Where(x => x.Status == "A").ToListAsync();

            return earningLines.Select(item => new PayrollEarningLine
            {
                DefId = item.DefId,
                EarningLineCode = item.EarningLineCode,
                EarningLineDescription = item.EarningLineDescription,
                Formula = item.Formula,
                Status = item.Status
            }).ToList();
        }

        public async Task<List<PayrollDeductionLine>> GetPayrollDeductionLines()
        {
            var deductionLines = await _dbContext.PayrollDeductionDef
                .Where(x => x.Status == "A").ToListAsync();

            return deductionLines.Select(item => new PayrollDeductionLine
            {
                DefId = item.DefId,
                DeductionCode = item.DeductionCode,
                DeductionDescription = item.DeductionDecsription,
                Formula = item.Formula,
                Status = item.Status
            }).ToList();
        }

        public async Task<ResponseModel> CreateEarningLine(PayrollEarningLine model)
        {
            if (string.IsNullOrEmpty(model.EarningLineCode) || string.IsNullOrEmpty(model.EarningLineDescription))
            {
                return ResponseEntity.GetResponse(ResponseConstants.RequiredDataNotProvided, 500, false);
            }

            var recordCount = await _dbContext.PayrollEarningDef.Where(x =>
                x.EarningLineCode == model.EarningLineCode ||
                x.EarningLineDescription!.Contains(model.EarningLineDescription)
            ).CountAsync();

            if (recordCount != 0)
            {
                return ResponseEntity.GetResponse(ResponseConstants.RecordAlreadyExists, 500, false);
            }

            var earningDef = new PayrollEarningDef()
            {
                EarningLineCode = model.EarningLineCode.ToUpper().Trim(),
                EarningLineDescription = model.EarningLineDescription.Trim(),
                Formula = model.Formula!.Trim(),
                Status = "A",
                DateCreated = DateTime.Now,
                LineFlag = 1,
                CreatedBy = _customLogger.GetCurrentUser()
            };
            await _dbContext.PayrollEarningDef.AddAsync(earningDef);
            await _dbContext.SaveChangesAsync();
            return ResponseEntity.GetResponse(ResponseConstants.RecordAddedSuccessfully, 200, true);
        }

        public async Task<ResponseModel> CreateDeductionLine(PayrollDeductionLine model)
        {
            if (string.IsNullOrEmpty(model.DeductionCode) || string.IsNullOrEmpty(model.DeductionDescription))
            {
                return ResponseEntity.GetResponse(ResponseConstants.RequiredDataNotProvided, 500, false);
            }

            var recountCount = await _dbContext.PayrollDeductionDef.Where(x =>
                x.DeductionCode == model.DeductionCode ||
                x.DeductionDecsription!.Contains(model.DeductionDescription)
            ).CountAsync();

            if (recountCount != 0)
            {
                return ResponseEntity.GetResponse(ResponseConstants.RecordAlreadyExists, 500, false);
            }

            var earningDef = new PayrollDeductionDef
            {
                DeductionCode = model.DeductionCode.ToUpper().Trim(),
                DeductionDecsription = model.DeductionDescription.Trim(),
                Formula = model.Formula!.Trim(),
                Status = StatusCodes.A.ToString(),
                DateCreated = DateTime.Now,
                LineFlag = 2,
                CreatedBy = _customLogger.GetCurrentUser()
            };
            await _dbContext.PayrollDeductionDef.AddAsync(earningDef);
            await _dbContext.SaveChangesAsync();
            return ResponseEntity.GetResponse(ResponseConstants.RecordAddedSuccessfully, 200, true);
        }

        public async Task<ResponseModel> EditEarningLine(string status, string definitionCode, string description,
            string formula)
        {
            if (string.IsNullOrEmpty(status) || string.IsNullOrEmpty(definitionCode))
            {
                return ResponseEntity.GetResponse(ResponseConstants.RequiredDataNotProvided, 500, false);
            }

            var earningLine = await _dbContext.PayrollEarningDef.Where(x => x.EarningLineCode == definitionCode)
                .FirstOrDefaultAsync();
            if (earningLine == null)
            {
                return ResponseEntity.GetResponse(ResponseConstants.RecordNotFound, 500, false);
            }

            earningLine.Status = status.ToUpper().Trim();
            earningLine.EarningLineDescription = description.Trim();
            earningLine.Formula = formula?.Trim();
            earningLine.DateModified = DateTime.Now;
            earningLine.ModifiedBy = _customLogger.GetCurrentUser();
            await _dbContext.SaveChangesAsync();

            return ResponseEntity.GetResponse(ResponseConstants.RecordEditedSuccessfully, 200, true);
        }

        public async Task<ResponseModel> EditDeductionLine(string status, string definitionCode, string description,
            string formula)
        {
            if (string.IsNullOrEmpty(status) || string.IsNullOrEmpty(definitionCode))
            {
                return ResponseEntity.GetResponse(ResponseConstants.RequiredDataNotProvided, 500, false);
            }

            var deductionLine = await _dbContext.PayrollDeductionDef.Where(x => x.DeductionCode == definitionCode)
                .FirstOrDefaultAsync();
            if (deductionLine == null)
            {
                return ResponseEntity.GetResponse(ResponseConstants.RecordNotFound, 200, true);
            }

            deductionLine.Status = status.ToUpper().Trim();
            deductionLine.DeductionDecsription = description.Trim();
            deductionLine.Formula = formula?.Trim();
            deductionLine.DateModified = DateTime.Now;
            deductionLine.ModifiedBy = _customLogger.GetCurrentUser();
            await _dbContext.SaveChangesAsync();

            return ResponseEntity.GetResponse(ResponseConstants.RecordEditedSuccessfully, 200, true);
        }

        public async Task<ResponseModel> CreatePayslipDefinition(PayrollDefinitionModel definitionModel)
        {
            if (!definitionModel.Code!.Any() || string.IsNullOrEmpty(definitionModel.EmployeeCode))
            {
                throw new Exception(ResponseConstants.RequiredDataNotProvided);
            }

            if (!definitionModel.Category!.Contains(ResponseConstants.Deductions) &&
                !definitionModel.Category.Contains(ResponseConstants.Earnings))
            {
                throw new Exception(ResponseConstants.InvalidPayrollDefinition);
            }

            var employeeCode = definitionModel.EmployeeCode
                .Split(" ")[2]
                .Replace("(", "").Replace(")", "").Trim().ToUpper();

            var employee = await _employeeObjectRepository.GetActiveEmployee(employeeCode);

            if (employee == null)
            {
                return ResponseEntity.GetResponse(ResponseConstants.EmployeeNotFound, 500, false);
            }

            var flagId = definitionModel.Category.Contains(ResponseConstants.Earnings) ? 1 : 2;
            var user = _customLogger.GetCurrentUser();
            var res = await AddPayslipDefinitionLine(definitionModel, employee.EmployeeId, flagId, user);

            return ResponseEntity.GetResponse(res, 200, true);
        }

        private async Task<string> AddPayslipDefinitionLine(PayrollDefinitionModel definitionModel, int employeeId, int flagId, string? userId)
        {
            var recordCounter = 0;
            var sb = new StringBuilder();

            for (var i = 0; i < definitionModel.Code!.Length; i++)
            {
                var code = definitionModel.Code[i];
                var description = definitionModel.Description![i];
                var value = definitionModel.Value![i];
                var occurenceFrequency = definitionModel.OccurenceCode![i];
                var type = definitionModel.Type![i];
                var currentDate = DateTime.Now;
                var firstDateOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
                var lastDateOfMonth = firstDateOfMonth.AddMonths(1).AddDays(-1);

                if (await LineExist(employeeId, code, flagId))
                {
                    sb.Append(code + ", ");
                }
                else
                {
                    await _dbContext.AddAsync(new PayslipDefinition
                    {
                        EmployeeId = employeeId,
                        PayrollDefinitionCode = code,
                        Description = description,
                        Type = type,
                        Value = value,
                        OccurenceCode = occurenceFrequency,
                        PeriodStartDate = firstDateOfMonth,
                        PeriodEndDate = lastDateOfMonth,
                        PayPeriod = DateTime.Now.Month,
                        DateModified = DateTime.Now,
                        UserId = userId,
                        PayrollDefinitionFlag = flagId,
                    });
                    await _dbContext.SaveChangesAsync();
                    recordCounter++;
                }
            }

            if (sb.Length != 0)
            {
                return sb + " already exists for this employee." + recordCounter + " record(s) uploaded successfully";
            }

            return recordCounter + " record(s) added successfully!";
        }

        private async Task<bool> LineExist(int employeeId, string code, int flagId)
        {
            var recordCount = await _dbContext.PayslipDefinition
                .Where(x => x.EmployeeId == employeeId
                            && (x.PayrollDefinitionCode == code)
                            && x.PayrollDefinitionFlag == flagId)
                .CountAsync();
            return recordCount > 0;
        }
    }
}