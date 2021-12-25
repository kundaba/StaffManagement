using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CDFStaffManagement.Enums;
using CDFStaffManagement.Interfaces;
using CDFStaffManagement.Services.Timesheet.Dto;
using CDFStaffManagement.Utilities;
using Microsoft.AspNetCore.Http;

namespace CDFStaffManagement.Controllers
{
    public class TimesheetController : Controller
    {
        private readonly ITimesheetService _timesheetService;
        private readonly ICustomLogger _customLogger;
        private readonly UserAuthentication _userAuthentication;

        public TimesheetController(ITimesheetService timesheetService, ICustomLogger customLogger,
            IHttpContextAccessor httpContextAccessor)
        {
            _timesheetService = timesheetService;
            _customLogger = customLogger;
            _userAuthentication = new UserAuthentication(httpContextAccessor);
        }

        public async Task<IActionResult> Index()
        {
            if (!_userAuthentication.IsSessionActive())
            {
                return RedirectToAction("Login", "UserAccount");
            }

            try
            {
                var timeSheets = await _timesheetService.GetUnApprovedTimesheet();
                return View(timeSheets);
            }
            catch (Exception ex)
            {
                _customLogger.ErrorLog(ex.Message + ex.InnerException);
                return RedirectToAction("ExceptionHandler", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddTimesheetHours(TimesheetSubmissionDto request)
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
                    await _timesheetService.AddTimesheetHours(request);
                return Json(response);
            }
            catch (Exception ex)
            {
                _customLogger.ErrorLog(ex.Message + ex.InnerException);
                return Json(ResponseEntity.GetResponse(ex.Message, 500, false));
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> ApproveTimesheet(IList<int> idList, string action)
        {
            if (!_userAuthentication.IsSessionActive())
            {
                return RedirectToAction("Login", "UserAccount");
            }

            if (!idList.Any() || string.IsNullOrEmpty(action))
            {
                return Json(ResponseEntity.GetResponse(ResponseConstants.RequiredDataNotProvided, 500, false));
            }

            try
            {
                var actionId = action.Equals("Approved") ? 1 : 0;
                var response =
                    await _timesheetService.ApproveTimesheet(idList, actionId);
                return Json(response);
            }
            catch (Exception ex)
            {
                _customLogger.ErrorLog(ex.Message + ex.InnerException);
                return Json(ResponseEntity.GetResponse(ex.Message, 500, false));
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> GetTimesheetByPeriod(DateTime startDate, DateTime endDate)
        {
            if (!_userAuthentication.IsSessionActive())
            {
                return RedirectToAction("Login", "UserAccount");
            }

            if (startDate.Equals(null) || endDate.Equals(null))
            {
                return Json(ResponseEntity.GetResponse(ResponseConstants.RequiredDataNotProvided, 500, false));
            }

            try
            {
                var response =
                    await _timesheetService.GetTimesheetByPeriod(startDate, endDate);
                return Json(response);
            }
            catch (Exception ex)
            {
                _customLogger.ErrorLog(ex.Message + ex.InnerException);
                return Json(ResponseEntity.GetResponse(ex.Message, 500, false));
            }
        }
    }
}