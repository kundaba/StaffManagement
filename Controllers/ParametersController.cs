using System;
using System.Threading.Tasks;
using CDFStaffManagement.Enums;
using CDFStaffManagement.Interfaces;
using CDFStaffManagement.Services.Parameters;
using CDFStaffManagement.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CDFStaffManagement.Controllers
{
    public class ParametersController : Controller
    {
        private readonly IParameterRepository _paramRepository;
        private readonly UserAuthentication _userAuthentication;
        private readonly ICustomLogger _logger;

        public ParametersController(IParameterRepository repository, IHttpContextAccessor httpContextAccessor,
            ICustomLogger logger)
        {
            _paramRepository = repository;
            _userAuthentication = new UserAuthentication(httpContextAccessor);
            _logger = logger;
        }

        public async Task<IActionResult> Department()
        {
            if (!_userAuthentication.IsSessionActive())
            {
                return RedirectToAction("Login", "UserAccount");
            }

            try
            {
                var departmentList = await _paramRepository.GetDepartmentList();
                return View(departmentList);
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message);
                return RedirectToAction("ExceptionHandler", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddDepartment(Departments model)
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
                var result = await _paramRepository.AddDepartment(model);
                return Json(result);
            }
            catch (Exception e)
            {
                _logger.ErrorLog(e.Message);
                return RedirectToAction("ExceptionHandler", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditDepartment(Departments model)
        {
            if (!_userAuthentication.IsSessionActive())
            {
                return RedirectToAction("Login", "UserAccount");
            }

            if (!ModelState.IsValid || string.IsNullOrEmpty(model.Status))
            {
                return Json(ResponseEntity.GetResponse(ResponseConstants.RequiredDataNotProvided, 500, false));
            }

            try
            {
                var addMsg = await _paramRepository.EditDepartment(model);
                return Json(addMsg);
            }
            catch (Exception e)
            {
                _logger.ErrorLog(e.Message);
                return RedirectToAction("ExceptionHandler", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            if (!_userAuthentication.IsSessionActive())
            {
                return RedirectToAction("Login", "UserAccount");
            }

            if (string.IsNullOrEmpty(id.ToString()))
            {
                return Json("Something went wrong..Please try again!");
            }

            try
            {
                var deleteMsg = await _paramRepository.DisableDepartment(id);
                return Json(deleteMsg);
            }
            catch (Exception e)
            {
                _logger.ErrorLog(e.Message);
                return RedirectToAction("ExceptionHandler", "Home");
            }
        }

        public async Task<IActionResult> JobTitles()
        {
            if (!_userAuthentication.IsSessionActive())
            {
                return RedirectToAction("Login", "UserAccount");
            }

            try
            {
                var jobTitles = await _paramRepository.GetJobTitleList();
                return View(jobTitles);
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message);
                return RedirectToAction("ExceptionHandler", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddJobTitle(JobTitles model)
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
                var addMsg = await _paramRepository.AddJobTitle(model);
                return Json(addMsg);
            }
            catch (Exception e)
            {
                _logger.ErrorLog(e.Message);
                return RedirectToAction("ExceptionHandler", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditJobTitle(JobTitles model)
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
                var addMsg = await _paramRepository.EditJobTitle(model);
                return Json(addMsg);
            }
            catch (Exception e)
            {
                _logger.ErrorLog(e.Message);
                return RedirectToAction("ExceptionHandler", "Home");
            }
        }

        public async Task<IActionResult> JobGrade()
        {
            if (!_userAuthentication.IsSessionActive())
            {
                return RedirectToAction("Login", "UserAccount");
            }

            try
            {
                var jobGrades = await _paramRepository.GetJobGradeList();
                return View(jobGrades);
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message);
                return RedirectToAction("ExceptionHandler", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddJobGrade(JobGrades model)
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
                var addMsg = await _paramRepository.AddJobGrade(model);
                return Json(addMsg);
            }
            catch (Exception e)
            {
                _logger.ErrorLog(e.Message);
                return RedirectToAction("ExceptionHandler", "Home");
            }
        }

        [HttpPost]
        public async Task<JsonResult> EditJobGrade(int gradeId, string description, string? status)
        {
            return !string.IsNullOrEmpty(gradeId.ToString()) &&
                   !string.IsNullOrEmpty(description) &&
                   !string.IsNullOrEmpty(status)
                ? Json(await _paramRepository.EditJobGrade(gradeId, description, status))
                : Json(ResponseConstants.SomethingWentWrong);
        }

        public async Task<IActionResult> Bank()
        {
            if (!_userAuthentication.IsSessionActive())
            {
                return RedirectToAction("Login", "UserAccount");
            }

            try
            {
                var isAjaxRequest = HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
                if (isAjaxRequest)
                {
                    return Json(await _paramRepository.BankList());
                }

                var bankList = await _paramRepository.BankList();
                return View(bankList);
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message);
                return RedirectToAction("ExceptionHandler", "Home");
            }
        }

        public async Task<IActionResult> BankBranch()
        {
            if (!_userAuthentication.IsSessionActive())
            {
                return RedirectToAction("Login", "UserAccount");
            }

            try
            {
                var branchList = await _paramRepository.BranchList();
                return View(branchList);
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message);
                return RedirectToAction("ExceptionHandler", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddBank(Banks model)
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
                var addMsg = await _paramRepository.AddBank(model);
                return Json(addMsg);
            }
            catch (Exception e)
            {
                _logger.ErrorLog(e.Message);
                return RedirectToAction("ExceptionHandler", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddBranch(Branch model)
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
                var addMsg = await _paramRepository.AddBranch(model);
                return Json(addMsg);
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message);
                return RedirectToAction("ExceptionHandler", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditBankName(Banks model)
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
                var editMsg = await _paramRepository.EditBank(model);
                return Json(editMsg);
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message);
                return RedirectToAction("ExceptionHandler", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditBranch(Branch model)
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
                var editMsg = await _paramRepository.EditBranch(model);
                return Json(editMsg);
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message);
                return RedirectToAction("ExceptionHandler", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetLookupData(string dataCategory)
        {
            if (!_userAuthentication.IsSessionActive())
            {
                return RedirectToAction("Login", "UserAccount");
            }

            if (string.IsNullOrEmpty(dataCategory))
            {
                return null!;
            }

            var data = await _paramRepository.GetLookupData(dataCategory);
            return Json(data);
        }

        [HttpPost]
        public async Task<IActionResult> GetBranchByBankId(int bankId)
        {
            if (!_userAuthentication.IsSessionActive())
            {
                return RedirectToAction("Login", "UserAccount");
            }

            if (string.IsNullOrEmpty(bankId.ToString()))
            {
                throw new Exception("Invalid bank id");
            }

            try
            {
                var data = await _paramRepository.GetBranchByBankId(bankId);
                return Json(data);
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message);
                return RedirectToAction("ExceptionHandler", "Home");
            }
        }

        public async Task<IActionResult> TaxDefinition()
        {
            if (!_userAuthentication.IsSessionActive())
            {
                return RedirectToAction("Login", "UserAccount");
            }

            try
            {
                var taxTable = await _paramRepository.GetTaxDefinitionList();
                return View(taxTable);
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message);
                return RedirectToAction("ExceptionHandler", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddEditTaxDefinition(TaxDefinition model, string task)
        {
            if (!_userAuthentication.IsSessionActive())
            {
                return RedirectToAction("Login", "UserAccount");
            }

            if (!ModelState.IsValid)
            {
                return Json(ResponseConstants.RequiredDataNotProvided);
            }

            var res = new ResponseModel();
            try
            {
                res = task switch
                {
                    "Add" => await _paramRepository.AddTaxDefinitionDetail(model),
                    "Edit" => await _paramRepository.EditTaxDefinitionDetail(model),
                    _ => res
                };

                return Json(res);
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message);
                return RedirectToAction("ExceptionHandler", "Home");
            }
        }
    }
}