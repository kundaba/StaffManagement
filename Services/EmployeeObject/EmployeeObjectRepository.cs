using System.Linq;
using System.Threading.Tasks;
using CDFStaffManagement.Interfaces;
using CDFStaffManagement.Model.DBContext;
using CDFStaffManagement.Model.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace CDFStaffManagement.Services.EmployeeObject
{
    public class EmployeeObjectRepository : IEmployeeObjectRepository
    {
        private readonly MyPayrollContext _dbContext;

        public EmployeeObjectRepository(MyPayrollContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<EmployeeDetail> GetActiveEmployee(string employeeCode)
        {
            return await _dbContext.EmployeeDetail
                .Where(x => x.EmployeeCode == employeeCode && x.StatusId != 5)
                .FirstOrDefaultAsync();
        }
        
        public async Task<EmployeeDetail> GetTerminatedEmployee(string employeeCode)
        {
            return await _dbContext.EmployeeDetail
                .Where(x => x.EmployeeCode == employeeCode && x.StatusId == 5)
                .FirstOrDefaultAsync();
        }
    }
}