
using System.Diagnostics;
using CDFStaffManagement.Enums;
using CDFStaffManagement.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CDFStaffManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserAuthentication _userAuthentication;

        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _userAuthentication = new UserAuthentication(httpContextAccessor);
        }
        public IActionResult Index()
        {
            if (!_userAuthentication.IsSessionActive())
            {
                return RedirectToAction("Login", "UserAccount");
            }
            ViewBag.UserName = TempData[ResponseConstants.UserName]?.ToString();
            return View();
        }
        public IActionResult NotAuthorised()
        {
            return View();
        }
        public IActionResult ExceptionHandler()
        {
            return View();
        }
        public IActionResult LoginError()
        {
            return View();
        }
    }
}
