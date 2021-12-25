using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CDFStaffManagement.Interfaces;
using CDFStaffManagement.Model.DBContext;
using CDFStaffManagement.Services.StoredProcedures;
using Microsoft.EntityFrameworkCore;

namespace CDFStaffManagement.Services.UserAccount
{
    public class UserManager : IUserManagerRepository
    {
        private readonly MyPayrollContext _dbContext;

        public UserManager(MyPayrollContext context)
        {
            _dbContext = context;
        }
        public IAsyncEnumerable<UserList> GetUserList()
        {
            try
            {
                var userList = _dbContext.GetUserList.FromSqlRaw("UserList")?.AsAsyncEnumerable();
                return userList ?? throw new InvalidOperationException();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<string> EditUserDetail(EditUserDetail model)
        {
            string msg;
            try
            {
                if(model.UserId != 0 && model.UserRole != 0 && model.EmailAddress != null)
                {
                    var user = await _dbContext.UserDetail.Where(x => x.UserId == model.UserId).FirstOrDefaultAsync();
                    if(user != null)
                    {
                        user.UserRoleId = model.UserRole;
                        user.EmailAddress = model.EmailAddress.Trim();
                        await _dbContext.SaveChangesAsync();
                    }
                    msg = "Details Edited Successfully";
                }
                else
                {
                    msg = "Please provide all the required details";
                }
            }
            catch(Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
        }
        public async Task<string> DisableActivateUser(int userId, string actionType)
        {
            if (string.IsNullOrEmpty(userId.ToString()) || string.IsNullOrEmpty(actionType))
            {
                return "Something went wrong. Please try again!";
            }
            var msg = "";
            try
            {
                var statusId = actionType switch
                {
                    "Disable" => 2,
                    "Activate" => 1,
                    _ => 0
                };
                var user = await _dbContext.UserDetail.Where(x => x.UserId == userId).FirstOrDefaultAsync();
                if (user != null)
                {
                    user.ProfileStatus = statusId;
                    await _dbContext.SaveChangesAsync();
                }
                msg = "User "+user!.Username +" Successfully "+actionType+"d";
            }
            catch(Exception ex)
            {
                msg = ex.Message;
            }

            return msg;
        }
    }
}
