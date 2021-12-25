using System;
using System.Threading.Tasks;
using CDFStaffManagement.Enums;
using CDFStaffManagement.Interfaces;
using CDFStaffManagement.Services.NapsaConfiguration;
using CDFStaffManagement.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace CDFStaffManagement.Controllers
{
    public class NapsaConfigurationController : Controller
    {
        private readonly INapsaConfigurationRepository _napsaConfigurationRepository;
        private readonly UserAuthentication _userAuthentication;
        private readonly ICustomLogger _logger;

        public NapsaConfigurationController(INapsaConfigurationRepository napsaConfiguration, IHttpContextAccessor httpContextAccessor, ICustomLogger logger)
        {
            _napsaConfigurationRepository = napsaConfiguration;
            _userAuthentication = new UserAuthentication(httpContextAccessor);
            _logger = logger;
        }

        public async Task<IActionResult> Configurations()
        {
            if (!_userAuthentication.IsSessionActive())
            {
                return RedirectToAction("Login", "UserAccount");
            }

            try
            {
                var configurations = await _napsaConfigurationRepository.GetNapsaConfigurations();
                return View(configurations);
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message);
                return RedirectToAction("ExceptionHandler", "Home");
            }
        }       

        public IActionResult Test()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> TestConfigurations()
        {
            if (!_userAuthentication.IsSessionActive())
            {
                return RedirectToAction("Login", "UserAccount");
            }

            try
            {
                var configurations = await _napsaConfigurationRepository.GetNapsaConfigurations();
                return Json(configurations);
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message);
                return RedirectToAction("ExceptionHandler", "Home");
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateNewLine(NapsaConfigurationDto request)
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
                var response = await _napsaConfigurationRepository.AddNewConfiguration(request);
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
