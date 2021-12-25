using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using CDFStaffManagement.Enums;
using CDFStaffManagement.Interfaces;
using CDFStaffManagement.Services.UserAccount;
using Microsoft.AspNetCore.Mvc;

namespace CDFStaffManagement.Controllers
{
    public class UserAccountController : Controller
    {
        private readonly IUserAccountRepository _userRepository;
        private readonly ICustomLogger _logger;

        public UserAccountController(IUserAccountRepository userAccountRepository, ICustomLogger logger)
        {
            _userRepository = userAccountRepository;
            _logger = logger;
        }
        public IActionResult Login()
        {
            TempData["LoginStatus"] = TempData["message"]?.ToString();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var isUserAuthenticated = await _userRepository.UserAuthenticated(model);
                if (isUserAuthenticated)
                {
                    if (await _userRepository.IsPasswordResetActive(model.UserName))
                    {
                        TempData["Username"] = model.UserName;
                        return RedirectToAction("ChangePassword");
                    }

                    TempData["Username"] = model.UserName;
                    return RedirectToAction("Index", "Home");
                }
                TempData["message"] = ResponseConstants.WrongUserNameOrPassword;
                    return RedirectToAction("Login");
               
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message + ex.InnerException);
                return RedirectToAction("LoginError", "Home");
            }
        }
        public IActionResult SignUp()
        {
            TempData["SignUpMessage"] = TempData["message"]?.ToString();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp([NotNull] SignUpViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                if (!_userRepository.IsPasswordValid(model.Password))
                {
                    TempData["message"] = ResponseConstants.PasswordIsNotAlphanumeric;
                    return RedirectToAction("SignUp");
                }
                var empCode = model.EmployeeCode!
                    .Replace("(", "")
                    .Replace(")", "")
                    .Trim().Split(" ")[2].ToUpper();
                
                model.EmployeeCode = empCode;
                var signUpMsg = await _userRepository.SignUp(model);
                
                TempData["message"] = signUpMsg;
                return RedirectToAction("SignUp");
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message + ex.InnerException);
                return RedirectToAction("ExceptionHandler", "Home");
            }
        }
        [HttpGet]
        public IActionResult SignOut()
        {
            try
            {
               _userRepository.SignOut();
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message + ex.InnerException);
                return RedirectToAction("LoginError", "Home");
            }
        }
        public IActionResult PasswordReset()
        {
            TempData["ResetMessage"] = TempData["message"]?.ToString();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PasswordReset(ResetPasswordViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                if (!_userRepository.IsPasswordValid(model.Password))
                {
                    TempData["message"] = ResponseConstants.PasswordIsNotAlphanumeric;
                    return RedirectToAction("PasswordReset");
                }
                var resetMsg = await _userRepository.ResetPassword(model);
                TempData["message"] = resetMsg;
                return RedirectToAction("PasswordReset");
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message + ex.InnerException);
                throw;
            }
        }
        public IActionResult ChangePassword()
        {
            TempData["Username"] = TempData["Username"]?.ToString();
            TempData["PasswordChangeMsg"] = TempData["PasswordChangeMsg"]?.ToString(); ;
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePassword changePwrdModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["Username"] = changePwrdModel.UserName;
                return View(changePwrdModel);
            }

            try
            {
                if (!_userRepository.IsPasswordValid(changePwrdModel.Password))
                {
                    TempData["Username"] = changePwrdModel.UserName;
                    TempData["PasswordChangeMsg"] = ResponseConstants.PasswordIsNotAlphanumeric;
                    return RedirectToAction("ChangePassword");
                }

                await _userRepository.ChangePassword(changePwrdModel);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message + ex.InnerException);
                return RedirectToAction("LoginError", "Home");
            }
        }
        [HttpPost]
        public async Task<JsonResult> GetUserRole()
        {
            try
            {
                var userRole = await _userRepository.GetUserRole();
                return Json(userRole);
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message + ex.InnerException);
                return Json(ex);
            }
        }
        [HttpPost]
        public JsonResult DeleteCookie()
        {
            Response.Cookies.Delete(ResponseConstants.UserName);
            return Json("");
        }
    }
}