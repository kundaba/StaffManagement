using System;
using System.ComponentModel.DataAnnotations;

namespace CDFStaffManagement.Services.LeaveManagement.Dto
{
    public class LeaveTypesDto
    {
        [Required]
        public string? Code {get; set;}
        [Required]
        public string? LeaveTypeDescription {get; set;}
        [Required]
        public int Entitlement {get; set;}
        [Required]
        public string? ApplicableGender {get; set;}
        [Required]
        public string? BalanceBroughtForwardOption { get; set; }
        [Required]
        public string? Cycle {get; set;}

        public DateTime DateCreated {get; set;}

        public string? CreatedBy {get; set;}
    }
}