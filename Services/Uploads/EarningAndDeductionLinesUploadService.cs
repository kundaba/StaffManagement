using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CDFStaffManagement.Enums;
using CDFStaffManagement.Interfaces;
using CDFStaffManagement.Model.DBContext;
using CDFStaffManagement.Model.EntityModels;
using CDFStaffManagement.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace CDFStaffManagement.Services.Uploads
{
    public class EarningAndDeductionLinesUploadService : IEarningAndDeductionLinesUploadService
    {
        private readonly MyPayrollContext _dbContext;
        private readonly ICustomLogger _customLogger;
        private readonly IEmployeeObjectRepository _employeeObjectRepository;

        public EarningAndDeductionLinesUploadService(MyPayrollContext dbContext, ICustomLogger customLogger,
            IEmployeeObjectRepository employeeObjectRepository)
        {
            _dbContext = dbContext;
            _customLogger = customLogger;
            _employeeObjectRepository = employeeObjectRepository;
        }

        public async Task<ResponseModel> UploadPayslipLines(IFormFile file, int lineId)
        {
            if (file == null)
            {
                throw new Exception("File does not contain data");
            }

            await using MemoryStream ms = new();
            await file.CopyToAsync(ms);

            using ExcelPackage package = new(ms);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelWorksheet workSheet = package.Workbook.Worksheets.FirstOrDefault()!;
            var totalRows = workSheet.Dimension.Rows;

            var currentDate = DateTime.Now;
            var firstDateOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            var lastDateOfMonth = firstDateOfMonth.AddMonths(1).AddDays(-1);

            IList<PayslipDefinition> payslipDefinitions = new List<PayslipDefinition>();
            var recordCounter = 0;

            for (var i = 2; i <= totalRows; i++)
            {
                var employeeCode = workSheet.Cells[i, 1].Value.ToString()!;
                var lineCode = workSheet.Cells[i, 2].Value.ToString()!;
                var employee = await _employeeObjectRepository.GetActiveEmployee(employeeCode);

                if (employee == null)
                {
                    continue;
                }

                PayslipDefinition payslipDefinition = new()
                {
                    EmployeeId = employee.EmployeeId,
                    PayrollDefinitionCode = lineCode,
                    Description = workSheet.Cells[i, 3].Value.ToString()!.Trim(),
                    Type = workSheet.Cells[i, 4].Value.ToString()!.Trim(),
                    Value = Convert.ToDecimal(workSheet.Cells[i, 5].Value.ToString()),
                    OccurenceCode = workSheet.Cells[i, 6].Value.ToString()!.Trim(),
                    PeriodStartDate = firstDateOfMonth,
                    PeriodEndDate = lastDateOfMonth,
                    DateModified = DateTime.Now,
                    UserId = _customLogger.GetCurrentUser(),
                    PayrollDefinitionFlag = lineId
                };

                if (await LineExist(employee.EmployeeId, lineCode, lineId, firstDateOfMonth, lastDateOfMonth))
                {
                    continue;
                }

                payslipDefinitions.Add(payslipDefinition);
                recordCounter++;
            }

            if (!payslipDefinitions.Any())
            {
                return ResponseEntity.GetResponse("Please make sure that the lines do not already exist", 500, false);
            }

            await SaveUploadedData(payslipDefinitions);
            return ResponseEntity.GetResponse(recordCounter + ResponseConstants.PayslipLinesUploadedSuccessfully, 200,
                true);
        }

        private async Task SaveUploadedData(IList<PayslipDefinition> payslipDefinitions)
        {
            if (!payslipDefinitions.Any())
            {
                throw new Exception("Failed to upload the file");
            }

            _dbContext.PayslipDefinition.AddRange(payslipDefinitions);
            await _dbContext.SaveChangesAsync();
        }

        private async Task<bool> LineExist(int employeeId, string code, int flagId, DateTime startDate,
            DateTime endDate)
        {
            var recordCount = await _dbContext.PayslipDefinition
                .Where(x =>
                    x.EmployeeId == employeeId
                    && x.PayrollDefinitionCode == code
                    && x.PayrollDefinitionFlag == flagId
                    && x.PeriodStartDate == startDate
                    && x.PeriodEndDate == endDate)
                .CountAsync();
            return recordCount > 0;
        }
    }
}