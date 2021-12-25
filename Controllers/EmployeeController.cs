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
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeRepository;
        private readonly UserAuthentication _userAuthentication;
        private readonly ICustomLogger _logger;

        public EmployeeController(IEmployeeService employeeRepository, IHttpContextAccessor httpContextAccessor,
            ICustomLogger logger)
        {
            _employeeRepository = employeeRepository;
            _userAuthentication = new UserAuthentication(httpContextAccessor);
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (!_userAuthentication.IsSessionActive())
            {
                return RedirectToAction("Login", "UserAccount");
            }

            return View();
        }

        public IActionResult NewEmployee()
        {
            if (!_userAuthentication.IsSessionActive())
            {
                return RedirectToAction("Login", "UserAccount");
            }

            return View();
        }

        /**
         * This end-point is used to fetch employee details by employee code
         */
        public async Task<IActionResult> Details(string employeeCode)
        {
            if (!_userAuthentication.IsSessionActive())
            {
                return RedirectToAction("Login", "UserAccount");
            }

            if (string.IsNullOrEmpty(employeeCode))
            {
                return RedirectToAction("ExceptionHandler", "Home");
            }

            try
            {
                var employeeDetails = await _employeeRepository.GetEmployeeByEmployeeCode(employeeCode);
                return View(employeeDetails);
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message + ex.InnerException);
                return RedirectToAction("ExceptionHandler", "Home");
            }
        }

        /**
         * This end-point is used to add a new employee
         */
        [HttpPost]
        public async Task<IActionResult> AddNewEmployee(NewEmployee employee)
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
                var response = await _employeeRepository.AddNewEmployeeAsync(employee);
                return Json(response);
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message + ex.InnerException);
                return RedirectToAction("ExceptionHandler", "Home");
            }
        }

        /**
         * End-point for performing auto-complete search for employees
         */
        [HttpPost]
        public async Task<IActionResult> AutoCompleteSearch([NotNull] string searchTerm)
        {
            if (!_userAuthentication.IsSessionActive())
            {
                return RedirectToAction("Login", "UserAccount");
            }

            if (string.IsNullOrEmpty(searchTerm))
            {
                return Json(ResponseConstants.SearchCriteriaNotProvided);
            }

            var searchResults = await _employeeRepository.EmployeeAutoCompleteSearch(searchTerm);
            return Json(searchResults);
        }

        /**
         * This end-point is used to fetch an employee object by employee code
         */
        [HttpPost]
        public async Task<IActionResult> GetEmployeeById([NotNull] string employeeCode)
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

                var empCode = employeeCode.Split(' ')[0].Trim();
                var searchResults = await _employeeRepository.GetEmployeeByEmployeeCode(empCode);
                return Json(searchResults);
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message + ex.InnerException);
                return RedirectToAction("ExceptionHandler", "Home");
            }
        }

        /**
         * End-point for fetching employee list
         */
        [HttpPost]
        public async Task<IActionResult> GetEmployeesList()
        {
            if (!_userAuthentication.IsSessionActive())
            {
                return RedirectToAction("Login", "UserAccount");
            }

            try
            {
                var employeeList = await _employeeRepository.EmployeeList();
                return Json(ResponseEntity.GetResponse(ResponseConstants.Success, 200, true, employeeList));
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message + ex.InnerException);
                return RedirectToAction("ExceptionHandler", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CheckEmployeeExistence([NotNull] string employeeCode)
        {
            try
            {
                return string.IsNullOrEmpty(employeeCode)
                    ? Json(ResponseConstants.EmployeeCodeNotProvided)
                    : Json(await _employeeRepository.EmployeeExistenceCheck(employeeCode));
            }
            catch (Exception ex)
            {
                _logger.ErrorLog(ex.Message + ex.InnerException);
                return RedirectToAction("ExceptionHandler", "Home");
            }
        }

        /**
         * This end-point is used for amending employee's personal details
         */
        [HttpPost]
        public async Task<IActionResult> EmployeeAmendment(EmployeeAmendmentDto amendmentRequest)
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
                var response = await _employeeRepository.AmendEmployeeDetails(amendmentRequest);
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