using System;
using System.Threading.Tasks;
using CDFStaffManagement.Enums;
using CDFStaffManagement.Interfaces;
using CDFStaffManagement.Services.Parameters;
using CDFStaffManagement.Services.Parameters.Interfaces;
using CDFStaffManagement.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CDFStaffManagement.Controllers
{
    public class TerminationReasonController : Controller
    {

        private readonly ITerminationReasonService _terminationReasonService;
        private readonly UserAuthentication _userAuthentication;
        private readonly ICustomLogger _logger;

        public TerminationReasonController(ITerminationReasonService terminationReasonService, ICustomLogger logger, IHttpContextAccessor httpContextAccessor)
        {
            _terminationReasonService = terminationReasonService;
            _userAuthentication = new UserAuthentication(httpContextAccessor);
            _logger = logger;
        }

        public async Task<IActionResult> TerminationReasons()
        {
            if (!_userAuthentication.IsSessionActive())
            {
                return RedirectToAction("Login", "UserAccount");
            }

            try
            {
                var terminationReasons = await _terminationReasonService.GetTerminationReasons();
                return View(terminationReasons);
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message);
                return RedirectToAction("ExceptionHandler", "Home");
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> AddTerminationReason(TerminationReasons  terminationReason )
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
                var result = await _terminationReasonService.AddTerminationReason(terminationReason);
                return Json(result);
            }
            catch (Exception e)
            {
                _logger.ErrorLog(e.Message);
                return RedirectToAction("ExceptionHandler", "Home");
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> EditTerminationReason(TerminationReasons  terminationReason )
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
                var result = await _terminationReasonService.EditTerminationReason(terminationReason);
                return Json(result);
            }
            catch (Exception e)
            {
                _logger.ErrorLog(e.Message);
                return RedirectToAction("ExceptionHandler", "Home");
            }
        }
    }
}