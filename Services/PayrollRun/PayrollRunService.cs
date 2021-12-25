using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CDFStaffManagement.Interfaces;
using CDFStaffManagement.Model.DBContext;
using CDFStaffManagement.Model.EntityModels;
using Microsoft.EntityFrameworkCore;


namespace CDFStaffManagement.Services.PayrollRun
{
    public class PayrollRunService : IPayrollRunService
    {
        private readonly MyPayrollContext _dbContext;
        private readonly ICustomLogger _customLogger;

        public PayrollRunService(MyPayrollContext dbContext, ICustomLogger customLogger)
        {
            _dbContext = dbContext;
            _customLogger = customLogger;
        }

        public async Task<List<PayslipDefinition>> GeneratePayrollFile()
        {
            var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var payrollFile = await _dbContext.PayslipDefinition
                    .Where(x => x.PayPeriod == DateTime.Now.Month).ToListAsync();

                if (!payrollFile.Any())
                {
                    throw new Exception("Failed to retrieve payroll file data");
                }
                await CarryOverRecurringLines(payrollFile);
                await transaction.CommitAsync();
                return payrollFile;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }

        private async Task CarryOverRecurringLines(IList<PayslipDefinition> payslipDefinitions)
        {
            List<PayslipDefinition> payslipDefinitionList = new();

            if (payslipDefinitions.Any())
            {
                foreach (var line in payslipDefinitions)
                {
                    if (!line.OccurenceCode!.Equals("REC"))
                    {
                        continue;
                    }
                    var payslipDefinition = new PayslipDefinition
                    {
                        EmployeeId = line.EmployeeId,
                        PayrollDefinitionCode = line.PayrollDefinitionCode,
                        Description = line.Description,
                        Type = line.Type,
                        OccurenceCode = line.OccurenceCode,
                        Value = line.Value,
                        PeriodStartDate = line.PeriodStartDate,
                        PeriodEndDate = line.PeriodEndDate,
                        PayPeriod = line.PayPeriod,
                        PayrollDefinitionFlag = line.PayrollDefinitionFlag,
                        UserId = _customLogger.GetCurrentUser()

                    };
                    payslipDefinitionList.Add(payslipDefinition);
                }

                await _dbContext.PayslipDefinition.AddRangeAsync(payslipDefinitionList);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}