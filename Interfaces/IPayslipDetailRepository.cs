using System.Threading.Tasks;
using CDFStaffManagement.Enums;
using CDFStaffManagement.Services.PayslipDetails.Dtos;

namespace CDFStaffManagement.Interfaces
{
    public interface IPayslipDetailRepository
    {
        Task<ResponseModel> GetPayslipDetails(string employeeCode);
        
        Task<ResponseModel> GetEarningsAndDeductions(string employeeCode);

        Task<ResponseModel> AmendPayslipDetails(PayslipAmendmentDto amendmentRequest);
        Task<ResponseModel> RemovePayrollLine(PayslipAmendmentDto amendmentRequest);
    }
}