using CDFStaffManagement.Enums;
using Microsoft.AspNetCore.Http;
using MyPayroll.Enums;

namespace CDFStaffManagement.Utilities
{
    public class UserAuthentication
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserAuthentication( IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public bool IsSessionActive()
        {
            var cookieId = _httpContextAccessor.HttpContext!.Request.Cookies[ResponseConstants.UserName];
            return !string.IsNullOrEmpty(cookieId);
        }
        public static bool UserAuthorized(string userRole)
        {
            return userRole != "2";
        }
    }
}
