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
    public class EmployeeQualificationsController : Controller
    {
        private readonly IQualificationDocumentsService _qualificationDocumentsService;
        private readonly ICustomLogger _logger;
        private readonly UserAuthentication _userAuthentication;

        public EmployeeQualificationsController(IQualificationDocumentsService qualificationDocumentsService,
            ICustomLogger logger, IHttpContextAccessor httpContextAccessor)
        {
            _qualificationDocumentsService = qualificationDocumentsService;
            _userAuthentication = new UserAuthentication(httpContextAccessor);
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> UploadQualifications(QualificationSubmissionDto qualificationSubmissionDto)
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
                    await _qualificationDocumentsService.UploadEmployeeQualification(qualificationSubmissionDto);
                return Json(response);
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message + ex.InnerException);
                return Json(ResponseEntity.GetResponse(ex.Message, 500, false));
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> GetEmployeeQualifications(string employeeCode)
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

                var response =
                    await _qualificationDocumentsService.GeEmployeeQualifications(employeeCode);
                return Json(response);
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message + ex.InnerException);
                return Json(ResponseEntity.GetResponse(ex.Message, 500, false));
            }
        }
        
        [HttpGet]
        public async Task<FileResult> DownloadQualificationDocument(string documentGuid)
        {
            if (!_userAuthentication.IsSessionActive())
            {
                throw new Exception("Inactive user session");
            }

            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception(ResponseConstants.RequiredDataNotProvided);
                }

                var response =
                    await _qualificationDocumentsService.GetDocument(documentGuid);

                return File(response.DocumentContent, "application/octet-stream", response.DocumentName, true);
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message + ex.InnerException);
                return File(string.Empty, "application/octet-stream", null);
            }
        }
    }
}