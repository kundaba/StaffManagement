using Microsoft.AspNetCore.Mvc;

namespace CDFStaffManagement.Controllers
{
    public class EmployeeFinderController : Controller
    {
        public IActionResult Finder()
        {
            return View();
        }
    }
}