using System.Threading.Tasks;
using CDFStaffManagement.Model.EntityModels;

namespace CDFStaffManagement.Interfaces
{
    public interface ICustomLogger
    {
        void CreateAuditLog(UserAuditLogs userAuditLog);
        void ErrorLog(string errorMessage);
        string? GetCurrentUser();
        Task<UserDetail> GetUserByUserName(string? username);
        Task<int?> GetUserId();
    }
}
