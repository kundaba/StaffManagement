using System.Collections.Generic;
using System.Threading.Tasks;
using CDFStaffManagement.Model.EntityModels;

namespace CDFStaffManagement.Interfaces
{
    public interface IPayrollRunService
    {
        Task<List<PayslipDefinition>> GeneratePayrollFile();
    }
}