using System;
using System.IO;
using System.Threading.Tasks;
using CDFStaffManagement.Enums;
using CDFStaffManagement.Interfaces;
using CDFStaffManagement.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CDFStaffManagement.Controllers
{
    public class BulkUploadController : Controller
    {
        private readonly UserAuthentication _userAuthentication;
        private readonly ICustomLogger _logger;
        private readonly IEarningAndDeductionLinesUploadService _earningAndDeductionLinesUploadService;

        public BulkUploadController(ICustomLogger logger, IHttpContextAccessor httpContextAccessor, IEarningAndDeductionLinesUploadService earningAndDeductionLinesUploadService)
        {
            _logger = logger;
            _earningAndDeductionLinesUploadService = earningAndDeductionLinesUploadService;
            _userAuthentication = new UserAuthentication(httpContextAccessor);
        }
        
        [HttpPost]
        public async Task<IActionResult> UploadPayslipLines(IFormFile file, int lineId)
        {
            if (!_userAuthentication.IsSessionActive())
            {
                return RedirectToAction("Login", "UserAccount");
            }

            if (file is null || lineId is 0 or > 2)
            {
                return Json(ResponseEntity.GetResponse(ResponseConstants.RequiredDataNotProvided, 500, false));
            }
            
            FileInfo newFile = new(file.FileName);
            string fileExtension = newFile.Extension;

            if (!fileExtension.Contains(".xlsx"))
            {
                return Json(ResponseEntity.GetResponse("Invalid file type", 500, false));
            }
            try
            {
                var result = await _earningAndDeductionLinesUploadService.UploadPayslipLines(file, lineId);
                return Json(result);
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message + ex.InnerException);
                return RedirectToAction("ExceptionHandler", "Home");
            }
        }
    }
}