using System;
using System.ComponentModel.DataAnnotations;

namespace CDFStaffManagement.Services.Employee.Dto
{
    public class EmployeeRemunerationDto
    {
        [Required]
        public string? EmployeeCode { get; set; }
        [Required]
        public decimal RemunerationAmount { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public string? Reason { get; set; }
        
        public DateTime? EndDate { get; set; }
        
        public DateTime? DateCreated { get; set; }
        
        public string? UserId { get; set; }
    }
}