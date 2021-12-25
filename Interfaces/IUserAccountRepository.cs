using System.Threading.Tasks;
using CDFStaffManagement.Services.UserAccount;

namespace CDFStaffManagement.Interfaces
{
    public interface IUserAccountRepository
    {
        Task<bool> UserAuthenticated(LoginViewModel model);
        Task<string> SignUp(SignUpViewModel model);
        void SignOut();
        Task<bool> IsPasswordResetActive(string? userName);
        Task<string> ResetPassword(ResetPasswordViewModel model);
        Task ChangePassword(ChangePassword changePasswordModel);
        Task<string> GetUserRole();
        bool IsPasswordValid(string? password);
    }
}
