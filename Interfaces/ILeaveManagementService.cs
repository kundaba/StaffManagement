using System.Collections.Generic;
using System.Threading.Tasks;
using CDFStaffManagement.Enums;
using CDFStaffManagement.Services.LeaveManagement.Dto;

namespace CDFStaffManagement.Interfaces
{
    public interface ILeaveManagementService
    {
         Task<List<LeaveTypesDto>> GetAllLeaveTypes();
         Task<ResponseModel> AddLeaveType(LeaveTypesDto leaveTypesDto);
         Task<ResponseModel> AddAndUpdateLeaveDetail(LeaveDetailDto leaveDetailDto);
         Task<ResponseModel> GetLeaveDetailByEmployeeCode(string employeeCode);
    }
}