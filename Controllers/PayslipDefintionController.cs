using System;
using System.Threading.Tasks;
using CDFStaffManagement.Enums;
using CDFStaffManagement.Interfaces;
using CDFStaffManagement.Services.Parameters;
using CDFStaffManagement.Services.PayrollDefinitions;
using CDFStaffManagement.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CDFStaffManagement.Controllers
{
    public class PayslipDefinitionController : Controller
    {
        private readonly IPayslipDefinitionRepository _payslipDefinitionRepository;
        private readonly UserAuthentication _userAuthentication;
        private readonly ICustomLogger _logger;

        public PayslipDefinitionController(
            IPayslipDefinitionRepository payslipDefinitionRepository,
            IHttpContextAccessor httpContextAccessor,
            ICustomLogger logger)
        {
            _payslipDefinitionRepository = payslipDefinitionRepository;
            _userAuthentication = new UserAuthentication(httpContextAccessor);
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (_userAuthentication.IsSessionActive())
            {
                return View();
            }

            return RedirectToAction("Login", "UserAccount");
        }

        public async Task<IActionResult> EarningLines()
        {
            if (!_userAuthentication.IsSessionActive())
            {
                return RedirectToAction("Login", "UserAccount");
            }

            try
            {
                var isAjaxRequest = HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
                var earningLineList = await _payslipDefinitionRepository.GetPayrollEarningLines();
                return isAjaxRequest ? Json(earningLineList) : View(earningLineList);
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message + ex.InnerException);
                return RedirectToAction("ExceptionHandler", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayslipDefinitionDetail(PayrollDefinitionModel model)
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
                var result = await _payslipDefinitionRepository.CreatePayslipDefinition(model);
                return Json(result);
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message + ex.InnerException);
                return RedirectToAction("ExceptionHandler", "Home");
            }
        }

        public async Task<IActionResult> DeductionLines()
        {
            if (!_userAuthentication.IsSessionActive())
            {
                return RedirectToAction("Login", "UserAccount");
            }

            try
            {
                var isAjaxRequest = HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
                var deductionLineList = await _payslipDefinitionRepository.GetPayrollDeductionLines();
                if (isAjaxRequest)
                {
                    return Json(deductionLineList);
                }

                return View(deductionLineList);
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message + ex.InnerException);
                return RedirectToAction("ExceptionHandler", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateEarningLine(PayrollEarningLine model)
        {
            if (!_userAuthentication.IsSessionActive())
            {
                return RedirectToAction("Login", "UserAccount");
            }
            
            if (!ModelState.IsValid)
            {
                return Json(ResponseConstants.RequiredDataNotProvided);
            }
            try
            {
                var addMsg = await _payslipDefinitionRepository.CreateEarningLine(model);
                return Json(addMsg);
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message + ex.InnerException);
                return RedirectToAction("ExceptionHandler", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateDeductionLine(PayrollDeductionLine model)
        {
            if (!_userAuthentication.IsSessionActive())
            {
                return RedirectToAction("Login", "UserAccount");
            }
            
            if (!ModelState.IsValid)
            {
                return Json(ResponseConstants.RequiredDataNotProvided);
            }

            try
            {
                var addMsg = await _payslipDefinitionRepository.CreateDeductionLine(model);
                return Json(addMsg);
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message + ex.InnerException);
                return RedirectToAction("ExceptionHandler", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditEarningLine(string status, string definitionCode, string description, string formula)
        {
            
            if (!_userAuthentication.IsSessionActive())
            {
                return RedirectToAction("Login", "UserAccount");
            }
            
            if (string.IsNullOrEmpty(status) || string.IsNullOrEmpty(definitionCode) ||
                string.IsNullOrEmpty(description))
            {
                return Json(ResponseConstants.RequiredDataNotProvided);
            }

            try
            {
                var editMsg = await _payslipDefinitionRepository.EditEarningLine(status, definitionCode, description, formula);
                return Json(editMsg);
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message + ex.InnerException);
                return RedirectToAction("ExceptionHandler", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditDeductionLine(string status, string definitionCode, string description, string formula)
        {
            if (string.IsNullOrEmpty(status) || string.IsNullOrEmpty(definitionCode) ||
                string.IsNullOrEmpty(description))
            {
                return Json(ResponseConstants.RequiredDataNotProvided);
            }

            try
            {
                var editMsg = await _payslipDefinitionRepository.EditDeductionLine(status, definitionCode, description, formula);
                return Json(editMsg);
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message + ex.InnerException);
                return RedirectToAction("ExceptionHandler", "Home");
            }
        }
    }
}