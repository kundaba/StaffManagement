using System.Collections.Generic;
using System.Threading.Tasks;
using CDFStaffManagement.Services.StoredProcedures;
using CDFStaffManagement.Services.UserAccount;

namespace CDFStaffManagement.Interfaces
{
    public interface IUserManagerRepository
    {
        IAsyncEnumerable<UserList> GetUserList();
        Task<string> EditUserDetail(EditUserDetail model);
        Task<string> DisableActivateUser(int userId, string actionType);
    }
}
