using System;
using System.Threading.Tasks;
using CDFStaffManagement.Enums;
using CDFStaffManagement.Interfaces;
using CDFStaffManagement.Services.DisciplinaryCases.Dto;
using CDFStaffManagement.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace CDFStaffManagement.Controllers
{
    public class DisciplinaryCasesController : Controller
    {
        private readonly IDisciplinaryCaseService _disciplinaryCaseService;
        private readonly ICustomLogger _logger;
        private readonly UserAuthentication _userAuthentication;

        public DisciplinaryCasesController(IDisciplinaryCaseService disciplinaryCaseService, ICustomLogger logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _disciplinaryCaseService = disciplinaryCaseService;
            _logger = logger;
            _userAuthentication = new UserAuthentication(httpContextAccessor);
        }

        public IActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> GetDisciplinaryCase()
        {
            if (!_userAuthentication.IsSessionActive())
            {
                return RedirectToAction("Login", "UserAccount");
            }
            
            try
            {
                var response =
                    await _disciplinaryCaseService.GetDisciplinaryCases();
                return Json(response);
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message + ex.InnerException);
                return Json(ResponseEntity.GetResponse(ex.Message, 500, false));
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> GetDisciplinaryCaseByCaseId(int caseId)
        {
            if (!_userAuthentication.IsSessionActive())
            {
                return RedirectToAction("Login", "UserAccount");
            }
            if (caseId == 0)
            {
                return Json(ResponseEntity.GetResponse(ResponseConstants.CaseIdNotProvided, 500, false));
            }
            try
            {
                var response =
                    await _disciplinaryCaseService.GetDisciplinaryCaseByCaseId(caseId);
                return Json(response);
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message + ex.InnerException);
                return Json(ResponseEntity.GetResponse(ex.Message, 500, false));
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> AddDisciplinaryCase(DisciplinaryCaseDto request)
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
                    await _disciplinaryCaseService.AddDisciplinaryCase(request);
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