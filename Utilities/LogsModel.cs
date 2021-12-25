using System;
using System.Linq;
using System.Threading.Tasks;
using CDFStaffManagement.Enums;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MyPayroll.Enums;
using CDFStaffManagement.Interfaces;
using CDFStaffManagement.Model.DBContext;
using CDFStaffManagement.Model.EntityModels;
using CDFStaffManagement.Utilities;

namespace CDFStaffManagement.Utilities
{
    public class LogsModel : ICustomLogger
    {
        private readonly MyPayrollContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDataProtector _protector;

        public LogsModel(MyPayrollContext dbContext, IHttpContextAccessor httpContextAccessor,  IDataProtectionProvider dataProtectionProvider)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _protector = dataProtectionProvider.CreateProtector(DataProtectionSettings.RouteValue);
        }
        public void ErrorLog(string errorMessage)
        {
            var errorLog = new ErrorLog
            {
                ErrorDescription = errorMessage,
                UserId = "",
                DateLogged = DateTime.Now
            };
            _dbContext.ErrorLog.Add(errorLog);
            _dbContext.SaveChanges();
        }
        public void CreateAuditLog(UserAuditLogs userAuditLog)
        {
            var auditLog = new UserAuditLogs
            {
                Guid = Guid.NewGuid().ToString(),
                UserName = GetCurrentUser(),
                EmployeeId = userAuditLog.EmployeeId,
                ActionDate = DateTime.Now,
                ActionType = userAuditLog.ActionType,
                Action = userAuditLog.Action,
                FieldName = userAuditLog.FieldName,
                OldValue = userAuditLog.OldValue,
                NewValue = userAuditLog.NewValue
            };
            _dbContext.UserAuditLogs.Add(auditLog);
            _dbContext.SaveChanges();
        }
        
        public string? GetCurrentUser()
        {
            if (_httpContextAccessor.HttpContext == null) return string.Empty;
            var userName = _httpContextAccessor.HttpContext.Request.Cookies[ResponseConstants.UserName];
            return userName == null? "" : _protector.Unprotect(userName);
        }

        public async Task<UserDetail> GetUserByUserName(string? username)
        {
            var user = await _dbContext.UserDetail
                .Where(x => x.Username!.Equals(username))
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new Exception(ResponseConstants.UserNotFound);
            }
            return user;
        }

        public async Task<int?> GetUserId()
        {
            var username = GetCurrentUser();
            var user = !string.IsNullOrEmpty(username) ? await GetUserByUserName(username) : null;
            var userId = user?.UserId;

            return userId;
        }
    }
}
