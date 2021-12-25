using System;
using System.ComponentModel.DataAnnotations;

namespace CDFStaffManagement.Services.Employee.Dto
{
    public class EmployeeTerminationDto
    {
        [Required]
        public string? EmployeeCode { get; set; }
        
        public string? PositionCode { get; set; }
        
        [Required]
        public int TerminationReason { get; set; }
        
        [Required]
        public DateTime TerminationDate { get; set; }
    }
}