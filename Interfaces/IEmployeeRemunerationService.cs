using System.Threading.Tasks;
using CDFStaffManagement.Enums;
using CDFStaffManagement.Services.Employee.Dto;

namespace CDFStaffManagement.Interfaces
{
    public interface IEmployeeRemunerationService
    {
        Task<ResponseModel> GetEmployeeRemunerationHistory(string employeeCode);
        Task<ResponseModel> CreateRemunerationLine(EmployeeRemunerationDto request, int employeeId);
        Task<ResponseModel> UpdateEmployeeRemuneration(EmployeeRemunerationDto remunerationDto);
    }
}