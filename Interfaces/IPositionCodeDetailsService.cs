using System.Collections.Generic;
using System.Threading.Tasks;
using CDFStaffManagement.Enums;
using CDFStaffManagement.Services.PositionCodeDetails.Dtos;

namespace CDFStaffManagement.Interfaces
{
    public interface IPositionCodeDetailsService
    {
        Task<ResponseModel> GetPositionCodes();
        Task<List<string>> GetVacantPositionCode(string searchTerm);
        
        Task<List<string>> GetVacantAndFilledPositionCode(string searchTerm);
        Task<ResponseModel> CreatePositionCode(int numOfPositionCodes, string jobTitleCode);
        Task<ResponseModel> SaveCreatedPositionCode(PositionCodeDto positionCodeDto);
        Task<ResponseModel> LinkEmployeeToPositionCode(string employeeCode, string positionCode);

        Task<ResponseModel> LinkPositionToSupervisor(string positionCode, string reportsToPosition);
    }
}