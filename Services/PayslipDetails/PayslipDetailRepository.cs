using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CDFStaffManagement.Enums;
using CDFStaffManagement.Interfaces;
using CDFStaffManagement.Model.DBContext;
using CDFStaffManagement.Model.EntityModels;
using CDFStaffManagement.Services.PayslipDetails.Dtos;
using CDFStaffManagement.Utilities;

namespace CDFStaffManagement.Services.PayslipDetails
{
    public class PayslipDetailRepository : IPayslipDetailRepository
    {
        private readonly MyPayrollContext _payrollContext;
        private readonly IEmployeeObjectRepository _employeeObjectRepository;

        public PayslipDetailRepository(MyPayrollContext payrollContext, IEmployeeObjectRepository employeeObjectRepository)
        {
            _payrollContext = payrollContext;
            _employeeObjectRepository = employeeObjectRepository;
        }

        /**
         * This method is used for getting employee's payslip details
         */
        public async Task<ResponseModel> GetPayslipDetails(string employeeCode)
        {
            if (string.IsNullOrEmpty(employeeCode))
            {
                return ResponseEntity.GetResponse(ResponseConstants.RequiredDataNotProvided, 500, false);
            }

            var emp = await _employeeObjectRepository.GetActiveEmployee(employeeCode);

            if (emp == null)
            {
                return ResponseEntity.GetResponse(ResponseConstants.EmployeeNotFound, 500, false);
            }

            var payslipDetails =
                await _payrollContext.PayslipDetail.Where(x => x.EmployeeId == emp.EmployeeId).ToListAsync();

            if (payslipDetails == null)
            {
                return ResponseEntity.GetResponse(ResponseConstants.FailedToFetchPayslipDetails, 500, false);
            }

            var payslipDetailsDto = new PayslipDetailsDto {EmployeeDetail = emp};
            payslipDetailsDto.PayslipDetails!.AddRange(payslipDetails);

            return ResponseEntity.GetResponse(
                ResponseConstants.Success,
                200,
                true,
                payslipDetailsDto
            );
        }
        
        /**
         * Get earnings and deductions
         */
        public async Task<ResponseModel> GetEarningsAndDeductions(string employeeCode)
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

            var payslipDetails =
                await _payrollContext.PayslipDefinition.Where(x => x.EmployeeId == employee.EmployeeId).ToListAsync();

            var earningsAndDeductionsList = await EarningsAndDeductionsList(payslipDetails, employee);

            return ResponseEntity.GetResponse(
                ResponseConstants.Success,
                200,
                true,
                earningsAndDeductionsList
            );
        }

        /**
         * Method for amending payslip details
         */
        public async Task<ResponseModel> AmendPayslipDetails(PayslipAmendmentDto amendmentRequest)
        {
            var employeeCode = amendmentRequest.EmployeeCode!.Replace("(", "").Replace(")", "").Trim().ToUpper();

            var employee = await _employeeObjectRepository.GetActiveEmployee(employeeCode);

            if (employee == null)
            {
                return ResponseEntity.GetResponse(ResponseConstants.EmployeeNotFound, 500, false);
            }

            var flag = amendmentRequest.PayrollDefinitionType!.Equals(ResponseConstants.Earnings) ? 1 : 2;

            var payrollLine = await _payrollContext.PayslipDefinition.Where(x =>
                x.EmployeeId == employee.EmployeeId && x.PayrollDefinitionCode == amendmentRequest.Code &&
                x.PayrollDefinitionFlag == flag).FirstOrDefaultAsync();

            if (payrollLine == null)
            {
                return ResponseEntity.GetResponse(ResponseConstants.PayrollLineNotFound, 500, false);
            }
            payrollLine.Value = amendmentRequest.AmendedValue;
            await _payrollContext.SaveChangesAsync();
            return ResponseEntity.GetResponse(ResponseConstants.RecordEditedSuccessfully, 200, true);
        }
        
        /**
         * This method is used for removing a given payroll line from an employee
         */
        public async Task<ResponseModel> RemovePayrollLine(PayslipAmendmentDto amendmentRequest)
        {
            var employeeCode = amendmentRequest.EmployeeCode!.Replace("(", "").Replace(")", "").Trim().ToUpper();

            var employee = await _employeeObjectRepository.GetActiveEmployee(employeeCode);

            if (employee == null)
            {
                return ResponseEntity.GetResponse(ResponseConstants.EmployeeNotFound, 500, false);
            }

            var payrollLine = await _payrollContext.PayslipDefinition.Where(x =>
                    x.EmployeeId == employee.EmployeeId && x.PayrollDefinitionCode == amendmentRequest.Code)
                .FirstOrDefaultAsync();

            if (payrollLine == null)
            {
                return ResponseEntity.GetResponse(ResponseConstants.PayrollLineNotFound, 500, false);
            }

            _payrollContext.Remove(payrollLine);
            await _payrollContext.SaveChangesAsync();
            return ResponseEntity.GetResponse(ResponseConstants.PayrollLineRemoved, 200, true);
        }
        private async Task<List<EarningsAndDeductionsDto>> EarningsAndDeductionsList(IEnumerable<PayslipDefinition> payslipDetails, EmployeeDetail employee)
        {
            var earningsAndDeductionsList = new List<EarningsAndDeductionsDto>();
            var amount = (decimal) 0.0;
            var totalEarnings = employee.BasicPay;

            var payslipDefinitions = payslipDetails.ToList();

            if (payslipDefinitions.Any())
            {
                foreach (var item in payslipDefinitions)
                {
                    amount = item.Type switch
                    {
                        ResponseConstants.Percentage => employee.BasicPay * (item.Value / 100),
                        ResponseConstants.FixedAmount => item.Value,
                        _ => amount
                    };

                    if (item.PayrollDefinitionFlag == 1)
                    {
                        totalEarnings += amount;
                    }

                    var earningsAndDeductions = new EarningsAndDeductionsDto
                    {
                        PayrollDefinitionCode = item.PayrollDefinitionCode!,
                        Description = item.Description!,
                        Type = item.Type!,
                        Value = item.Value,
                        Amount = amount,
                        DateModified = item.DateModified,
                        PayrollDefinitionFlag = item.PayrollDefinitionFlag,
                    };
                    earningsAndDeductionsList.Add(earningsAndDeductions);
                }
            }

            var napsa = await GetNapsa(totalEarnings);
            var basicPay = GetBasicPay(employee.BasicPay);
            var nhima = await CalculateNationalHealthInsurance(employee.BasicPay);
            earningsAndDeductionsList.Add(basicPay);
            earningsAndDeductionsList.Add(await CalculatePayAsYouEarn(totalEarnings));
            earningsAndDeductionsList.Add(napsa);
            earningsAndDeductionsList.Add(nhima);
            return earningsAndDeductionsList;
        }

        private static EarningsAndDeductionsDto GetBasicPay(decimal basicPay)
        {
           return new EarningsAndDeductionsDto
           {
               PayrollDefinitionCode = "BASIC",
               Description = "Basic Pay",
               Type = ResponseConstants.FixedAmount,
               Value = basicPay,
               Amount = basicPay,
               DateModified = null,
               PayrollDefinitionFlag = 1,
           };
        }
        private async Task<EarningsAndDeductionsDto> GetNapsa(decimal grossPay)
        {
            decimal percentage;
            decimal napsaDeductibleAmount;

            var activeNapsaLine = await _payrollContext.NapsaConfiguration
                .Where(x => x.EndDate == null || x.EndDate > DateTime.Now)
                .FirstAsync();

            if (activeNapsaLine == null)
            {
                percentage = 0;
                napsaDeductibleAmount = 0;
            }
            else
            {
                napsaDeductibleAmount = (activeNapsaLine.Percentage / 100) * grossPay;
                percentage = activeNapsaLine.Percentage;

                if (napsaDeductibleAmount >= activeNapsaLine.MaximumCeiling)
                {
                    napsaDeductibleAmount = activeNapsaLine.MaximumCeiling;
                }
            }

            return new EarningsAndDeductionsDto
            {
                PayrollDefinitionCode = "NAPSA",
                Description = "NAPSA",
                Type = ResponseConstants.Percentage,
                Value = percentage,
                Amount = napsaDeductibleAmount,
                DateModified = null,
                PayrollDefinitionFlag = 2,
            };
        }
        private async Task<EarningsAndDeductionsDto> CalculateNationalHealthInsurance(decimal basicPay)
        {
            decimal percentage;
            decimal nhimaDeductibleAmount;

            var activeNhimaLine = await _payrollContext.NhimaConfiguration
                .Where(x => x.EndDate == null || x.EndDate > DateTime.Now)
                .FirstAsync();

            if (activeNhimaLine == null)
            {
                percentage = 0;
                nhimaDeductibleAmount = 0;
            }
            else
            {
                percentage = activeNhimaLine.Percentage;
                nhimaDeductibleAmount = percentage / 100 * basicPay;
            }

            return new EarningsAndDeductionsDto
            {
                PayrollDefinitionCode = "NHIMA",
                Description = "National Health Insurance",
                Type = ResponseConstants.Percentage,
                Value = percentage,
                Amount = nhimaDeductibleAmount,
                DateModified = null,
                PayrollDefinitionFlag = 2
            };
        }
        private async Task<EarningsAndDeductionsDto> CalculatePayAsYouEarn(decimal grossPay)
        {
            if (grossPay <= 0)
            {
                return new EarningsAndDeductionsDto();
            }

            var taxTable = await _payrollContext.TaxTableDefinition
                .OrderBy(x => x.LowerLimit)
                .Where(x => x.Status == StatusCodes.A.ToString() && (x.EndDate == null || x.EndDate > DateTime.Now))
                .ToListAsync();

            if (!taxTable.Any())
            {
                throw new Exception("Failed to retrieve tax table information");
            }

            var taxDeductibleValue = GetTaxDeductibleValue(grossPay, taxTable);

            return new EarningsAndDeductionsDto()
            {
                PayrollDefinitionCode = "PAYE",
                Description = "Pay As You Earn",
                Type = ResponseConstants.FixedAmount,
                Value = taxDeductibleValue,
                Amount = taxDeductibleValue,
                DateModified = null,
                PayrollDefinitionFlag = 2,
            };
        }

        private static decimal GetTaxDeductibleValue(decimal grossPay, IReadOnlyCollection<TaxTableDefinition> taxTable)
        {
            var firstBand = taxTable.ElementAt(0)?.Amount == null ? new decimal(0.0) : taxTable.ElementAt(0)?.Amount;
            var secondBand = taxTable.ElementAt(1)?.Amount == null ? new decimal(0.0) : taxTable.ElementAt(1)?.Amount;
            var thirdBand = taxTable.ElementAt(2)?.Amount == null ? new decimal(0.0) : taxTable.ElementAt(2)?.Amount;
            var fourthBand = (grossPay - taxTable.ElementAt(3)?.LowerLimit) * (taxTable.ElementAt(3)?.Percentage / 100);

            var deductibleAmount = new decimal?(new decimal(0.0));

            if (grossPay > 0 && grossPay <= taxTable.ElementAt(0)?.UperLimit)
            {
                deductibleAmount = firstBand;
            }

            if (grossPay >= taxTable.ElementAt(1)?.LowerLimit && grossPay <= taxTable.ElementAt(1)?.UperLimit)
            {
                deductibleAmount = firstBand + secondBand;
            }

            if (grossPay >= taxTable.ElementAt(2)?.LowerLimit && grossPay <= taxTable.ElementAt(2)?.UperLimit)
            {
                deductibleAmount = firstBand + secondBand + thirdBand;
            }

            if (grossPay >= taxTable.ElementAt(3)?.LowerLimit)
            {
                deductibleAmount = firstBand + secondBand + thirdBand + fourthBand;
            }

            var deductibleValue = decimal.Round(Convert.ToDecimal(deductibleAmount), 2, MidpointRounding.AwayFromZero);
            return deductibleValue;
        }
    }
}