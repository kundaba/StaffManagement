using System;
using System.Linq;
using System.Threading.Tasks;
using CDFStaffManagement.Enums;
using CDFStaffManagement.Interfaces;
using CDFStaffManagement.Model.DBContext;
using CDFStaffManagement.Model.EntityModels;
using CDFStaffManagement.Utilities;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CDFStaffManagement.Services.UserAccount
{
    public class UserAccountRepository : IUserAccountRepository
    {
        private readonly IDataProtector _protector;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly MyPayrollContext _dbContext;
        private readonly EmailSender _emailSender;
        private readonly IOptions<AppSettings> _appSettings;

        public UserAccountRepository(MyPayrollContext dbContext,
            IDataProtectionProvider dataProtectionProvider,
            IHttpContextAccessor httpContextAccessor,
            IOptions<AppSettings> appSettings,
            IOptions<EmailSettings> emailSettings)
        {
            _protector = dataProtectionProvider.CreateProtector(DataProtectionSettings.RouteValue);
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
            _emailSender = new EmailSender(emailSettings);
            _appSettings = appSettings;
        }

        private void SetCookie(string key, string? value)
        {
            var option = new CookieOptions();
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value)) return;
            option.Expires = DateTime.Now.AddMinutes(_appSettings.Value.CookieDuration);
            _httpContextAccessor.HttpContext!.Response.Cookies.Append(key, value, option);
        }

        private void DeleteCookie(string key)
        {
            _httpContextAccessor.HttpContext!.Response.Cookies.Delete(key);
        }

        private string? GetCookie(string key)
        {
            return _httpContextAccessor.HttpContext!.Request.Cookies[key];
        }

        public bool IsPasswordValid(string? password)
        {
            var isPasswordValid = PasswordValidator.IsValidPassword(password);
            return isPasswordValid;
        }

        public async Task<bool> UserAuthenticated(LoginViewModel model)
        {
            if (model == null)
            {
                return false;
            }

            var userCount = 1;
            try
            {
                var password = await GetUserPassword(model);
                var decryptedPassword = !string.IsNullOrEmpty(password)
                    ? StringProtector(password, ResponseConstants.Decrypt)
                    : "nil";

                 userCount = await _dbContext.UserDetail
                    .Where(user => user.Username == model.UserName
                                   && (decryptedPassword == model.Password)
                                   && (user.ProfileStatus == 1)
                    ).CountAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            if (userCount > 0 )
            {
                var user = await _dbContext.UserDetail.SingleOrDefaultAsync(x => x.Username == model.UserName);
                user.FailedLoginAttempts = 0;
                user.LastLogon = DateTime.Now;
                await _dbContext.SaveChangesAsync();
                var cookieValue = StringProtector(model.UserName, ResponseConstants.Encrypt);
                DeleteCookie(ResponseConstants.UserName);
                SetCookie(ResponseConstants.UserName, cookieValue);
                return true;
            }
            else
            {
                var failedLoginAttempts = await GetFailedLoginAttempts(model.UserName);
                var user = await _dbContext.UserDetail.SingleOrDefaultAsync(x => x.Username == model.UserName);
                if (user == null) return false;
                if (failedLoginAttempts == 3)
                {
                    user.ProfileStatus = 3;
                }

                user.FailedLoginAttempts = failedLoginAttempts + 1;
                user.LastLogon = DateTime.Now;
                await _dbContext.SaveChangesAsync();
                return false;
            }
        }

        private async Task<string?> GetUserPassword(LoginViewModel model)
        {
            return await _dbContext.UserDetail
                .Where(x => x.Username == model.UserName)
                .Select(x => x.Password)
                .FirstOrDefaultAsync() ?? throw new InvalidOperationException();
        }

        public async Task<string> SignUp(SignUpViewModel model)
        {
            try
            {
                if (await UserExists(model.EmployeeCode!))
                {
                    return ResponseConstants.UserNameAlreadyTaken;
                }

                if (!await EmployeeExists(model.EmployeeCode!))
                {
                    return ResponseConstants.EmployeeCodeNotFound;
                }

                await SaveSignUpDetailsAsync(model);
                return ResponseConstants.UserAccountCreatedSuccessfully;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        public void SignOut()
        {
            var cookieValue = GetCookie(ResponseConstants.UserName);
            if (!string.IsNullOrEmpty(cookieValue))
            {
                DeleteCookie(ResponseConstants.UserName);
            }
        }

        public async Task<string> ResetPassword(ResetPasswordViewModel model)
        {
            var userDetail = await
            (
                from userAccount in _dbContext.UserDetail
                where (userAccount.Username == model.UserName)
                      && (userAccount.EmailAddress == model.Email)
                select userAccount
            ).FirstOrDefaultAsync();

            if (userDetail == null)
            {
                return ResponseConstants.UserNameDoesNotExist;
            }

            if (userDetail.ProfileStatus != 1)
            {
                return "User is not active";
            }

            var encryptedPassword = StringProtector(model.Password, ResponseConstants.Encrypt);
            var user = await _dbContext.UserDetail.SingleOrDefaultAsync(x => x.Username == model.UserName);
            user.Password = encryptedPassword;
            user.ProfileStatus = 1;
            user.FailedLoginAttempts = 0;
            await _dbContext.SaveChangesAsync();
            await CreatePasswordResetToken(model.UserName!);
            try
            {
                await _emailSender.SendEmailAsync(model.Email!, "Password Reset",
                    "<b>Dear " + model.UserName + ",</b>\n Your " + ResponseConstants.PasswordResetSuccessful);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return ResponseConstants.PasswordResetSuccessful;
        }

        private string? StringProtector(string? data, string action)
        {
            var protectedString = action switch
            {
                ResponseConstants.Encrypt => _protector.Protect(data!),
                ResponseConstants.Decrypt => _protector.Unprotect(data!),
                _ => ""
            };

            return protectedString;
        }

        private async Task<int> GetFailedLoginAttempts(string? userName)
        {
            var failedLoginAttempts = Convert.ToInt32(
                await _dbContext.UserDetail
                    .Where(x => x.Username == userName)
                    .Select(x => x.FailedLoginAttempts)
                    .FirstOrDefaultAsync()
            );
            return failedLoginAttempts;
        }

        private async Task<bool> UserExists(string userName)
        {
            var userCount = await
            (
                from user in _dbContext.UserDetail
                where user.Username == userName
                select user
            ).CountAsync();
            return userCount > 0;
        }

        private async Task<bool> EmployeeExists(string employeeCode)
        {
            var recordCount = await
            (
                from employee in _dbContext.Employee
                where employee.EmployeeCode == employeeCode && employee.EmployeeStatusId != 5
                select employee
            ).CountAsync();
            return recordCount > 0;
        }

        private async Task SaveSignUpDetailsAsync(SignUpViewModel model)
        {
            var password = StringProtector(model.Password, ResponseConstants.Encrypt);

            var employee = await _dbContext.Employee
                .Where(x => x.EmployeeCode == model.EmployeeCode && x.EmployeeStatusId != 5)
                .FirstOrDefaultAsync();

            if (employee == null)
            {
                throw new Exception(ResponseConstants.EmployeeNotFound);
            }

            var userDetail = new UserDetail
            {
                EmployeId = employee.EmployeeId,
                Username = model.UserName,
                EmailAddress = model.Email,
                Password = password,
                UserRoleId = model.UserRole,
                ProfileStatus = 1,
                DateCreated = DateTime.Now,
                FailedLoginAttempts = 0
            };
            await _dbContext.UserDetail.AddAsync(userDetail);
            await _dbContext.SaveChangesAsync();
            if (_dbContext.SaveChangesAsync().IsCompletedSuccessfully)
            {
                await CreatePasswordResetToken(model.UserName!);
            }
        }

        private async Task CreatePasswordResetToken(string userName)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                await _dbContext.UserPasswordResets.AddAsync(new UserPasswordResets
                    {
                        UserId = await _dbContext.UserDetail
                            .Where(x => x.Username == userName)
                            .Select(x => x.UserId).FirstOrDefaultAsync(),
                        UserName = userName,
                        ResetDate = DateTime.Now,
                        ResetToken = Guid.NewGuid().ToString(),
                        TokenStatusId = 1
                    }
                );
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> IsPasswordResetActive(string? userName)
        {
            var count = await
            (
                from user in _dbContext.UserPasswordResets
                where user.UserName == userName
                      && user.TokenStatusId == 1
                select user
            ).CountAsync();
            return count > 0;
        }

        public async Task ChangePassword(ChangePassword changePasswordModel)
        {
            if (string.IsNullOrEmpty(changePasswordModel.UserName))
            {
                throw new Exception(ResponseConstants.UserNameNotProvided);
            }

            var encryptedPassword = StringProtector(changePasswordModel.Password, ResponseConstants.Encrypt);

            var user = await _dbContext.UserDetail.Where(x =>
                x.Username == changePasswordModel.UserName).FirstOrDefaultAsync();

            if (user == null)
            {
                throw new Exception(ResponseConstants.UserNotFound);
            }

            user.Password = encryptedPassword;
            user.ProfileStatus = 1;
            user.FailedLoginAttempts = 0;
            await _dbContext.SaveChangesAsync();

            var passwordResetToken = await _dbContext.UserPasswordResets
                .Where(x => x.UserName == changePasswordModel.UserName).FirstOrDefaultAsync();

            if (passwordResetToken != null)
            {
                passwordResetToken.TokenStatusId = 0;
                await _dbContext.SaveChangesAsync();
                //await _emailSender.SendEmailAsync(user.EmailAddress, "Password Reset", "Dear " + changePasswordModel.UserName + " Your Password has successfully been Changed.");
            }
        }

        public async Task<string> GetUserRole()
        {
            var userName = StringProtector(GetCookie(ResponseConstants.UserName), ResponseConstants.Decrypt);
            var userRole = Convert.ToString(await _dbContext.UserDetail
                .Where(x => x.Username == userName && x.ProfileStatus == 1)
                .Select(x => x.UserRoleId)
                .FirstOrDefaultAsync());
            return !string.IsNullOrEmpty(userRole) ? userRole : "UserNotFound";
        }
    }
}