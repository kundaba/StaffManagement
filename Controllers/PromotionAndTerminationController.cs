using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using CDFStaffManagement.Enums;
using CDFStaffManagement.Interfaces;
using CDFStaffManagement.Services.Employee.Dto;
using CDFStaffManagement.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CDFStaffManagement.Controllers
{
    public class PromotionAndTerminationController : Controller
    {
        private readonly UserAuthentication _userAuthentication;
        private readonly IPromotionAndTerminationService _employeeTerminationService;
        private readonly ICustomLogger _logger;

        public PromotionAndTerminationController(ICustomLogger logger, IHttpContextAccessor httpContextAccessor, IPromotionAndTerminationService employeeTerminationService)
        {
            _logger = logger;
            _employeeTerminationService = employeeTerminationService;
            _userAuthentication = new UserAuthentication(httpContextAccessor);
        }
        
        /**
         * This end-point is used for terminating an employee
         */
        [HttpPost]
        public async Task<IActionResult> TerminateEmployee(EmployeeTerminationDto terminationRequest)
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
                var response = await _employeeTerminationService.TerminateEmployee(terminationRequest);
                return Json(response);
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message + ex.InnerException);
                return RedirectToAction("ExceptionHandler", "Home");
            }
        }
        /**
         * This end-point is used for terminating an employee
         */
        [HttpPost]
        public async Task<IActionResult> ReinstateEmployee([NotNull]string employeeCode)
        {
            if (!_userAuthentication.IsSessionActive())
            {
                return RedirectToAction("Login", "UserAccount");
            }

            if (string.IsNullOrEmpty(employeeCode))
            {
                return Json(ResponseEntity.GetResponse(ResponseConstants.RequiredDataNotProvided, 500, false));
            }

            try
            {
                var response = await _employeeTerminationService.ReinstateEmployee(employeeCode);
                return Json(response);
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message + ex.InnerException);
                return RedirectToAction("ExceptionHandler", "Home");
            }
        }
        
        /**
         * This end-point is used for promoting/transferring an employee
         */
        [HttpPost]
        public async Task<IActionResult> PromoteEmployee(PromotionAndTransferDto promotionRequest)
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
                var response = await _employeeTerminationService.PromoteEmployee(promotionRequest);
                return Json(response);
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message + ex.InnerException);
                return RedirectToAction("ExceptionHandler", "Home");
            }
        }
    }
}