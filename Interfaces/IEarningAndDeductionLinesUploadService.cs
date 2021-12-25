using System.Threading.Tasks;
using CDFStaffManagement.Enums;
using Microsoft.AspNetCore.Http;
using MyPayroll.Enums;

namespace CDFStaffManagement.Interfaces
{
    public interface IEarningAndDeductionLinesUploadService
    {
        Task<ResponseModel> UploadPayslipLines(IFormFile file, int lineId);
    }
}