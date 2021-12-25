using System;
using System.ComponentModel.DataAnnotations;

namespace CDFStaffManagement.Services.Employee.Dto
{
    public class PromotionAndTransferDto
    {
        [Required]
        public string? EmployeeCode { get; set; }
        [Required]
        public string? OldPositionCode { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public string? NewPositionCode { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
    }
}