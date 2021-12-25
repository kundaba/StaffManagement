using System;
using System.Threading.Tasks;
using CDFStaffManagement.Enums;
using CDFStaffManagement.Interfaces;
using CDFStaffManagement.Services.Employee.Dto;
using CDFStaffManagement.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace CDFStaffManagement.Controllers
{
    public class BasicPayAmendmentController : Controller
    {
        private readonly UserAuthentication _userAuthentication;
        private readonly IEmployeeRemunerationService _employeeRemunerationService;
        private readonly ICustomLogger _logger;

        public BasicPayAmendmentController(IHttpContextAccessor httpContextAccessor, IEmployeeRemunerationService employeeRemunerationService, ICustomLogger logger)
        {
            _userAuthentication = new UserAuthentication(httpContextAccessor);
            _employeeRemunerationService = employeeRemunerationService;
            _logger = logger;
        }

        /**
         * This end-point is used to fetch basic pay details
         */
        [HttpPost]
        public async Task<IActionResult> GetBasicPayHistory(string employeeCode)
        {
            if (!_userAuthentication.IsSessionActive())
            {
                return RedirectToAction("Login", "UserAccount");
            }

            try
            {
                if (string.IsNullOrEmpty(employeeCode))
                {
                    return Json(ResponseEntity.GetResponse(ResponseConstants.RequiredDataNotProvided, 500, false));
                }

                var searchResults =
                    await _employeeRemunerationService.GetEmployeeRemunerationHistory(employeeCode.Trim());
                return Json(searchResults);
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message + ex.InnerException);
                return Json(ResponseEntity.GetResponse(ex.Message, 500, false));
            }
        }

        /**
         * This end-point is used to update basic pay for a given employee
         */
        [HttpPost]
        public async Task<IActionResult> UpdateEmployeeRemuneration(EmployeeRemunerationDto request)
        {
            if (!_userAuthentication.IsSessionActive())
            {
                return RedirectToAction("Login", "UserAccount");
            }

            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(ResponseEntity.GetResponse(ResponseConstants.RequiredDataNotProvided, 500, false));
                }

                var response =
                    await _employeeRemunerationService.UpdateEmployeeRemuneration(request);
                return Json(response);
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message + ex.InnerException);
                return Json(ResponseEntity.GetResponse(ex.Message, 500, false));
            }
        }
    }
}
