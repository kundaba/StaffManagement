using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using CDFStaffManagement.Enums;
using CDFStaffManagement.Interfaces;
using CDFStaffManagement.Services.PositionCodeDetails.Dtos;
using CDFStaffManagement.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CDFStaffManagement.Controllers
{
    public class PositionCodesController : Controller
    {
        private readonly ICustomLogger _logger;
        private readonly UserAuthentication _userAuthentication;
        private readonly IPositionCodeDetailsService _positionCodeDetailsService;

        public PositionCodesController(ICustomLogger logger, IHttpContextAccessor httpContextAccessor, IPositionCodeDetailsService positionCodeDetailsService)
        {
            _logger = logger;
            _positionCodeDetailsService = positionCodeDetailsService;
            _userAuthentication  = new UserAuthentication(httpContextAccessor);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetPositionCodes()
        {
            if (!_userAuthentication.IsSessionActive())
            {
                return RedirectToAction("Login", "UserAccount");
            }

            try
            {
                var response = await _positionCodeDetailsService.GetPositionCodes();
                return Json(response);
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message + ex.InnerException);
                return Json(ResponseEntity.GetResponse(ex.Message, 500, false));
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> CreatePositionCode([NotNull] int numOfPositionCodes, [NotNull] string jobTitleCode )
        {
            if (!_userAuthentication.IsSessionActive())
            {
                return RedirectToAction("Login", "UserAccount");
            }
            try
            {
                if (string.IsNullOrEmpty(jobTitleCode) || numOfPositionCodes == 0)
                {
                    return Json(ResponseEntity.GetResponse(ResponseConstants.RequiredDataNotProvided, 500, false));
                }

                var response =
                    await _positionCodeDetailsService.CreatePositionCode(numOfPositionCodes, jobTitleCode);
                return Json(response);
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message + ex.InnerException);
                return Json(ex.Message);
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveCreatedPositionCode(PositionCodeDto request )
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
                    await _positionCodeDetailsService.SaveCreatedPositionCode(request);
                return Json(response);
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message + ex.InnerException);
                return Json(ex.Message);
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> SearchPositionCodes([NotNull] string searchTerm , [NotNull] string status)
        {
            if (!_userAuthentication.IsSessionActive())
            {
                return RedirectToAction("Login", "UserAccount");
            }
            try
            {
                if (string.IsNullOrEmpty(searchTerm) || string.IsNullOrEmpty(status))
                {
                    return Json(ResponseEntity.GetResponse(ResponseConstants.RequiredDataNotProvided, 500, false));
                }

                var response = status.Equals(ResponseConstants.Vacant)
                    ? await _positionCodeDetailsService.GetVacantPositionCode(searchTerm)
                    : await _positionCodeDetailsService.GetVacantAndFilledPositionCode(searchTerm);
                return Json(response);
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message + ex.InnerException);
                return Json(ex.Message);
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> LinkEmployeeToPositionCode([NotNull] string employeeCode, [NotNull] string positionCode )
        {
            if (!_userAuthentication.IsSessionActive())
            {
                return RedirectToAction("Login", "UserAccount");
            }

            try
            {
                if (string.IsNullOrEmpty(employeeCode) || string.IsNullOrEmpty(positionCode))
                {
                    return Json(ResponseEntity.GetResponse(ResponseConstants.RequiredDataNotProvided, 500, false));
                }

                positionCode = positionCode.Split("-")[0].Trim().ToUpper();
                var response =
                    await _positionCodeDetailsService.LinkEmployeeToPositionCode(employeeCode, positionCode);
                return Json(response);
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message + ex.InnerException);
                return Json(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> LinkPositionToSupervisor([NotNull] string positionCode, [NotNull] string reportsTo)
        {
            if (!_userAuthentication.IsSessionActive())
            {
                return RedirectToAction("Login", "UserAccount");
            }

            try
            {
                if (string.IsNullOrEmpty(positionCode) || string.IsNullOrEmpty(reportsTo))
                {
                    return Json(ResponseEntity.GetResponse(ResponseConstants.RequiredDataNotProvided, 500, false));
                }

                positionCode = positionCode.Split("-")[0].Trim().ToUpper();
                var response =
                    await _positionCodeDetailsService.LinkPositionToSupervisor(positionCode, reportsTo);
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