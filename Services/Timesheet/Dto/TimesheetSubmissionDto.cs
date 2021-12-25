using System;
using System.ComponentModel.DataAnnotations;

namespace CDFStaffManagement.Services.Timesheet.Dto
{
    public class TimesheetSubmissionDto
    {
        [Required]
        public string? EmployeeCode { get; set; }
        [Required]
        public decimal? HoursWorked { get; set; }
        [Required]
        public DateTime Date { get; set; }
    }
}