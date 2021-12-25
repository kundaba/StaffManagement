using System;
using System.Threading.Tasks;
using CDFStaffManagement.Enums;
using CDFStaffManagement.Interfaces;
using CDFStaffManagement.Services.PayslipDetails.Dtos;
using CDFStaffManagement.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CDFStaffManagement.Controllers
{
    public class PayslipDetailsController : Controller
    {
        private readonly IPayslipDetailRepository _payslipDetailRepository;
        private readonly UserAuthentication _userAuthentication;
        private readonly ICustomLogger _logger;

        public PayslipDetailsController(IPayslipDetailRepository payslipDetailRepository,
            IHttpContextAccessor httpContextAccessor, ICustomLogger logger)
        {
            _payslipDetailRepository = payslipDetailRepository;
            _userAuthentication = new UserAuthentication(httpContextAccessor);
            _logger = logger;
        }
        
        [HttpPost]
        public async Task<IActionResult> GetPayslipDetails(string employeeCode)
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
                employeeCode = employeeCode.Replace("(", "").Replace(")", "").Trim().ToUpper();
                var data = await _payslipDetailRepository.GetPayslipDetails(employeeCode);
                return Json(data);
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message);
                return RedirectToAction("ExceptionHandler", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EmployeeEarningsAndDeductions(string employeeCode)
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
                employeeCode = employeeCode.Replace("(", "").Replace(")", "").Trim().ToUpper();
                var data = await _payslipDetailRepository.GetEarningsAndDeductions(employeeCode);
                return Json(data);
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message);
                return RedirectToAction("ExceptionHandler", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EarningsAndDeductionsAmendment(PayslipAmendmentDto request)
        {
            if (!_userAuthentication.IsSessionActive())
            {
                return RedirectToAction("Login", "UserAccount");
            }

            if (!ModelState.IsValid)
            {
                return Json(ResponseEntity.GetResponse(ResponseConstants.RequiredDataNotProvided, 500, false));
            }

            if (request.PayrollDefinitionType!.Equals("Percentage") &&
                request.AmendedValue is < 1 or > 100)
            {
                return Json(ResponseEntity.GetResponse(ResponseConstants.RequiredDataNotProvided, 500, false)); 
            }
            try
            {
                var response = await _payslipDetailRepository.AmendPayslipDetails(request);
                return Json(response);
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message);
                return RedirectToAction("ExceptionHandler", "Home");
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> RemoveEmployeePayrollLine(PayslipAmendmentDto request)
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
                var response = await _payslipDetailRepository.RemovePayrollLine(request);
                return Json(response);
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message);
                return RedirectToAction("ExceptionHandler", "Home");
            }
        }
    }
}