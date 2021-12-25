
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CDFStaffManagement.Enums;
using CDFStaffManagement.Interfaces;
using CDFStaffManagement.Model.DBContext;
using CDFStaffManagement.Model.EntityModels;
using CDFStaffManagement.Services.Timesheet.Dto;
using CDFStaffManagement.Utilities;
using Microsoft.EntityFrameworkCore;
using MyPayroll.Enums;

namespace CDFStaffManagement.Services.Timesheet
{
    public class TimesheetService : ITimesheetService
    {
        private readonly MyPayrollContext _dbContext;
        private readonly IEmployeeObjectRepository _employeeObjectRepository;
        private readonly ICustomLogger _customLogger;

        public TimesheetService(MyPayrollContext dbContext, IEmployeeObjectRepository employeeObjectRepository,
            ICustomLogger customLogger)
        {
            _dbContext = dbContext;
            _employeeObjectRepository = employeeObjectRepository;
            _customLogger = customLogger;
        }

        /**
         * This method is used to fetch all unapproved timesheets
         */
        public async Task<List<TimesheetDto>> GetUnApprovedTimesheet()
        {
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<EmployeeTimeSheet, TimesheetDto>(); });
            var iMapper = config.CreateMapper();

            var timesheetDetails = await _dbContext.EmployeeTimeSheet
                .OrderByDescending(x => x.DateCreated)
                .Where(x => x.Status == TimesheetApprovalStatus.PendingApproval.ToString())
                .ToListAsync();

            var mappedList = timesheetDetails.Select(item => iMapper.Map<TimesheetDto>(item)).ToList();
            return mappedList;
        }

        public async Task<ResponseModel> GetTimesheetByPeriod(DateTime dateFrom, DateTime dateTo)
        {
            if (dateTo.Equals(null) || dateFrom.Equals(null))
            {
                ResponseEntity.GetResponse(ResponseConstants.RequiredDataNotProvided, 500, false);
            }

            var yearFrom = dateFrom.Year;
            var monthFrom = dateFrom.Month;
            var dayFrom = dateFrom.Day;
            var yearTo = dateTo.Year;
            var monthTo = dateTo.Month;
            var dayTo = dateTo.Day;

            var config = new MapperConfiguration(cfg => { cfg.CreateMap<EmployeeTimeSheet, TimesheetDto>(); });
            var iMapper = config.CreateMapper();

            var timesheetDetails = await _dbContext.EmployeeTimeSheet
                .OrderByDescending(x => x.DateCreated)
                .Where(x =>
                    x.DateCreated!.Value.Year >= yearFrom &&
                    x.DateCreated!.Value.Month >= monthFrom &&
                    x.DateCreated!.Value.Day >= dayFrom &&
                    x.DateCreated!.Value.Year <= yearTo &&
                    x.DateCreated!.Value.Month <= monthTo &&
                    x.DateCreated!.Value.Day <= dayTo)
                .ToListAsync();

            var mappedList = timesheetDetails.Select(item => iMapper.Map<TimesheetDto>(item)).ToList();
            return ResponseEntity.GetResponse(ResponseConstants.Success, 200, true, mappedList);
        }

        public async Task<ResponseModel> AddTimesheetHours(TimesheetSubmissionDto timesheetSubmissionDto)
        {
            if (timesheetSubmissionDto is null)
            {
                throw new Exception(ResponseConstants.EmployeeCodeNotFound);
            }

            var employee =
                await _employeeObjectRepository.GetActiveEmployee(timesheetSubmissionDto.EmployeeCode!.Trim());

            if (employee is null)
            {
                return ResponseEntity.GetResponse(ResponseConstants.EmployeeNotFound, 500, false);
            }

            var timesheetExist = await _dbContext.EmployeeTimeSheet
                .Where(x => x.EmployeeCode == timesheetSubmissionDto.EmployeeCode &&
                            x.DateWorked!.Value.Day == timesheetSubmissionDto.Date.Day)
                .FirstOrDefaultAsync();

            if (timesheetExist is not null)
            {
                return ResponseEntity.GetResponse(ResponseConstants.TimesheetAlreadyExist, 500, false);
            }

            return await SaveTimesheet(timesheetSubmissionDto, employee);
        }

        private async Task<ResponseModel> SaveTimesheet(TimesheetSubmissionDto timesheetSubmissionDto,
            EmployeeDetail? employee)
        {
            var firstDateOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var lastDateOfMonth = firstDateOfMonth.AddMonths(1).AddDays(-1);

            var timesheet = new EmployeeTimeSheet
            {
                EmployeeCode = timesheetSubmissionDto.EmployeeCode,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                HoursWorked = timesheetSubmissionDto.HoursWorked,
                DateWorked = timesheetSubmissionDto.Date,
                PeriodStartDate = firstDateOfMonth,
                PeriodEndDate = lastDateOfMonth,
                Status = TimesheetApprovalStatus.PendingApproval.ToString(),
                DateCreated = DateTime.Now
            };
            await _dbContext.EmployeeTimeSheet.AddAsync(timesheet);
            await _dbContext.SaveChangesAsync();
            return ResponseEntity.GetResponse(ResponseConstants.TimesheetAddedSuccessfully, 200, true);
        }

        public async Task<ResponseModel> ApproveTimesheet(IList<int> timesheetIds, int actionId)
        {
            if (!timesheetIds.Any())
            {
                return ResponseEntity.GetResponse(ResponseConstants.Error, 500, false);
            }

            string status = actionId == 1
                ? TimesheetApprovalStatus.Approved.ToString()
                : TimesheetApprovalStatus.Rejected.ToString();

            var counter = 0;
            foreach (var id in timesheetIds)
            {
                var timeSheet = await _dbContext.EmployeeTimeSheet.FindAsync(id);

                if (timeSheet is null)
                {
                    continue;
                }

                timeSheet.Status = status;
                timeSheet.DateApproved = DateTime.Now;
                timeSheet.ApprovedBy = _customLogger.GetCurrentUser();
                await _dbContext.SaveChangesAsync();
                counter++;
            }

            return counter > 0
                ? ResponseEntity.GetResponse(ResponseConstants.TimesheetApprovedSuccessfully, 200, true)
                : ResponseEntity.GetResponse(ResponseConstants.Error, 500, false);
        }
    }
}