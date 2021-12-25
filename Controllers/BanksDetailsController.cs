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
    public class BanksDetailsController : Controller
    {
        private readonly UserAuthentication _userAuthentication;
        private readonly IBanksDetailsService _banksDetailsService;
        private readonly ICustomLogger _logger;

        public BanksDetailsController(IBanksDetailsService banksDetailsService, ICustomLogger logger, IHttpContextAccessor httpContextAccessor)
        {
            _banksDetailsService = banksDetailsService;
            _userAuthentication = new UserAuthentication(httpContextAccessor);
            _logger = logger;
        }
        
        /**
         * This end-point is used to fetch bank details for a particular employee
         */
        [HttpPost]
        public async Task<IActionResult> GetEmployeeBankDetails(string employeeCode)
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
                    await _banksDetailsService.GetBankDetailsByEmployeeCode(employeeCode.Trim());
                return Json(searchResults);
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message + ex.InnerException);
                return Json(ResponseEntity.GetResponse(ex.Message, 500, false));
            }
        }
        
        /**
         * This end-point is used to update bank details for a given employee
         */
        [HttpPost]
        public async Task<IActionResult> UpdateBankDetails(BankDetailsViewDto request, int bankId, int branchId)
        {
            if (!_userAuthentication.IsSessionActive())
            {
                return RedirectToAction("Login", "UserAccount");
            }

            try
            {
                if (!ModelState.IsValid || bankId == 0 || branchId == 0)
                {
                    return Json(ResponseEntity.GetResponse(ResponseConstants.RequiredDataNotProvided, 500, false));
                }

                var response =
                    await _banksDetailsService.UpdateBankDetails(request, bankId, branchId);
                return Json(response);
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message + ex.InnerException);
                return Json(ResponseEntity.GetResponse(ex.Message, 500, false));
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> ChangeDefaultBank(string employeeCode, string accountNumber)
        {
            if (!_userAuthentication.IsSessionActive())
            {
                return RedirectToAction("Login", "UserAccount");
            }

            try
            {
                if (string.IsNullOrWhiteSpace(employeeCode) || string.IsNullOrWhiteSpace(accountNumber))
                {
                    return Json(ResponseEntity.GetResponse(ResponseConstants.RequiredDataNotProvided, 500, false));
                }

                var response =
                    await _banksDetailsService.ChangeDefaultBank(employeeCode, accountNumber);
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