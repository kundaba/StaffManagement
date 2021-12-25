using System.Threading.Tasks;
using CDFStaffManagement.Model.EntityModels;

namespace CDFStaffManagement.Interfaces
{
    public interface IEmployeeObjectRepository
    {
        Task<EmployeeDetail> GetActiveEmployee(string employeeCode);

        Task<EmployeeDetail> GetTerminatedEmployee(string employeeCode);
    }
}