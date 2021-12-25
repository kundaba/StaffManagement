using System.Threading.Tasks;
using CDFStaffManagement.Enums;
using CDFStaffManagement.Services.Employee.Dto;
using MyPayroll;

namespace CDFStaffManagement.Interfaces
{
    public interface IBanksDetailsService
    {
        Task<ResponseModel> SaveEmployeeBankDetails(EmployeeBankDetails bankDetails);
        Task<ResponseModel> GetBankDetailsByEmployeeCode(string employeeCode);
        Task<ResponseModel> UpdateBankDetails(BankDetailsViewDto bankDetailsViewDto, int bankId, int branchId);

        Task<ResponseModel> ChangeDefaultBank(string employeeCode, string accountNumber);
    }
}