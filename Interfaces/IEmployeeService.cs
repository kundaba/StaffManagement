using System.Collections.Generic;
using System.Threading.Tasks;
using CDFStaffManagement.Enums;
using CDFStaffManagement.Model.EntityModels;
using CDFStaffManagement.Services.Employee.Dto;

namespace CDFStaffManagement.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<string>> EmployeeAutoCompleteSearch(string searchTerm);
        Task<EmployeeDetail> GetEmployeeByEmployeeCode(string employeeCode);
        Task<bool> EmployeeExistenceCheck(string employeeCode);
        Task<ResponseModel> AddNewEmployeeAsync(NewEmployee newEmployee);
        Task<List<EmployeeDetail>> EmployeeList();
        Task<ResponseModel> AmendEmployeeDetails(EmployeeAmendmentDto amendmentDto);
        Task<List<EmployeeDetail>> GetEmployeesByStatus(IEnumerable<int> statusId);
    }
}
