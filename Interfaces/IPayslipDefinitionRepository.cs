using System.Collections.Generic;
using System.Threading.Tasks;
using CDFStaffManagement.Enums;
using CDFStaffManagement.Services.Parameters;
using CDFStaffManagement.Services.PayrollDefinitions;

namespace CDFStaffManagement.Interfaces
{
    public interface IPayslipDefinitionRepository
    {
        Task<List<PayrollEarningLine>> GetPayrollEarningLines();
        Task<List<PayrollDeductionLine>> GetPayrollDeductionLines();
        Task<ResponseModel> CreateEarningLine(PayrollEarningLine model);
        Task<ResponseModel> CreateDeductionLine(PayrollDeductionLine model);
        Task<ResponseModel> EditEarningLine(string status, string definitionCode, string description, string formula);
        Task<ResponseModel> EditDeductionLine(string status, string definitionCode, string description, string formula);
        Task<ResponseModel> CreatePayslipDefinition(PayrollDefinitionModel definitionModel);
    }
}
