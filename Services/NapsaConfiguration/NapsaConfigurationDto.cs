using System;
using System.ComponentModel.DataAnnotations;

namespace CDFStaffManagement.Services.NapsaConfiguration
{
    public class NapsaConfigurationDto
    {
        public int Id { get; set; }
        
        [Required]
        public decimal Percentage { get; set; } 
        [Required]
        public decimal MaximumCeiling { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
    }
}