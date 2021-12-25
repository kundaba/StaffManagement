using System;
using System.ComponentModel.DataAnnotations;

namespace CDFStaffManagement.Services.PositionCodeDetails.Dtos
{
    public class PositionCodeDto
    {
        [Required]
        public string? JobTitleCode { get; set; }
        [Required]
        public string? PositionCode { get; set; }
       
        public string? ShortDescription { get; set; }
    
        public string? LongDescription { get; set; }
        [Required]
        public int DepartmentId { get; set; }
        public string? ReportsToPositionCode { get; set; }
        
        public string? EmployeeCode { get; set; }
        public DateTime StartDate { get; set; }
        
        public string? Status { get; set; }
        
        public string? CreatedBy { get; set; }
        
        public DateTime CreatedDate { get; set; }
    }
}