using System.Threading.Tasks;
using CDFStaffManagement.Enums;
using CDFStaffManagement.Services.Employee.Dto;

namespace CDFStaffManagement.Interfaces
{
    public interface IPromotionAndTerminationService
    {
        Task<ResponseModel> TerminateEmployee(EmployeeTerminationDto terminationRequest);
        Task<ResponseModel> ReinstateEmployee(string employeeCode);
        Task<ResponseModel> PromoteEmployee(PromotionAndTransferDto promotionRequest);
    }
}