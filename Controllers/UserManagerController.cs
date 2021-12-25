
using System.Collections.Generic;
using System.Threading.Tasks;
using CDFStaffManagement.Interfaces;
using CDFStaffManagement.Services.StoredProcedures;
using CDFStaffManagement.Services.UserAccount;
using CDFStaffManagement.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CDFStaffManagement.Controllers
{
    public class UserManagerController : Controller
    {
        private readonly IUserManagerRepository _userMgr;
        private readonly UserAuthentication _userAuthentication;
        private readonly IUserAccountRepository _userRepository;
        public UserManagerController(IUserManagerRepository userManager, 
            IHttpContextAccessor httpContextAccessor,
            IUserAccountRepository userAccountRepository)
        {
            _userMgr = userManager;
            _userAuthentication = new UserAuthentication(httpContextAccessor);
            _userRepository = userAccountRepository;
        }
        public async  Task<IActionResult> UserList()
        {
            if (!_userAuthentication.IsSessionActive()) return RedirectToAction("Login", "UserAccount");
            var userRole = await _userRepository.GetUserRole();
            if (!UserAuthentication.UserAuthorized(userRole)) return RedirectToAction("NotAuthorised", "Home");
            var userList = _userMgr.GetUserList();
            return View((IEnumerable<UserList>) userList);
        }
        [HttpPost]
        public async Task<JsonResult> EditUserDetail(EditUserDetail model)
        {
            if(!ModelState.IsValid)
            {
                return Json("Please fill in both fields with correct values");
            }
            else
            {
                string editedRecord = await _userMgr.EditUserDetail(model);
                return Json(editedRecord);
            }
        }
        [HttpPost]
        public async Task<JsonResult> DisableActivateUser(int userId, string actionType)
        {
            if (!string.IsNullOrEmpty(userId.ToString()) && !string.IsNullOrEmpty(actionType))
            {
                return Json(await _userMgr.DisableActivateUser(userId,actionType));
            }
            return Json("Something went wrong");
        }
        [HttpPost]
        public IActionResult CheckUserSession()
        {
            bool isSessionActive = _userAuthentication.IsSessionActive();
            if (isSessionActive)
            {
                return Json("");
            }
            else
            {
                return RedirectToAction("Login", "UserAccount");
            }
        }
    }
}