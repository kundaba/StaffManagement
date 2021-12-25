using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CDFStaffManagement.Enums;
using CDFStaffManagement.Services.Timesheet.Dto;

namespace CDFStaffManagement.Interfaces
{
    public interface ITimesheetService
    {
        Task<List<TimesheetDto>> GetUnApprovedTimesheet();
        Task<ResponseModel> AddTimesheetHours(TimesheetSubmissionDto timesheetSubmissionDto);
        Task<ResponseModel> ApproveTimesheet(IList<int> timesheetIds, int actionId);
        Task<ResponseModel> GetTimesheetByPeriod(DateTime dateFrom, DateTime dateTo);
    }
}
