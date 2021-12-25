using System;
using System.Threading.Tasks;
using CDFStaffManagement.Enums;
using CDFStaffManagement.Interfaces;
using CDFStaffManagement.Services.LeaveManagement.Dto;
using CDFStaffManagement.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CDFStaffManagement.Controllers
{
    public class LeaveManagementController : Controller
    {
        private readonly ILeaveManagementService _leaveManagementService;
        private readonly ICustomLogger _logger;
        private readonly UserAuthentication _userAuthentication;

        public LeaveManagementController(ILeaveManagementService leaveManagementService, ICustomLogger logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _leaveManagementService = leaveManagementService;
            _userAuthentication = new UserAuthentication(httpContextAccessor);
            _logger = logger;
        }

        public async Task<IActionResult> LeaveTypes()
        {
            if (!_userAuthentication.IsSessionActive())
            {
                return RedirectToAction("Login", "UserAccount");
            }

            try
            {
                var leaveTypes = await _leaveManagementService.GetAllLeaveTypes();
                return View(leaveTypes);
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message + ex.InnerException);
                return RedirectToAction("ExceptionHandler", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddLeaveType(LeaveTypesDto request)
        {
            if (!_userAuthentication.IsSessionActive())
            {
                return RedirectToAction("Login", "UserAccount");
            }

            if (!ModelState.IsValid)
            {
                return Json(ResponseEntity.GetResponse(ResponseConstants.RequiredDataNotProvided, 500, false));
            }

            try
            {
                var response =
                    await _leaveManagementService.AddLeaveType(request);
                return Json(response);
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message + ex.InnerException);
                return Json(ResponseEntity.GetResponse(ex.Message, 500, false));
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetEmployeeLeaveDetail(string employeeCode)
        {
            if (!_userAuthentication.IsSessionActive())
            {
                return RedirectToAction("Login", "UserAccount");
            }

            if (string.IsNullOrEmpty(employeeCode))
            {
                return Json(ResponseEntity.GetResponse(ResponseConstants.EmployeeCodeNotProvided, 500, false));
            }

            try
            {
                var response =
                    await _leaveManagementService.GetLeaveDetailByEmployeeCode(employeeCode);
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