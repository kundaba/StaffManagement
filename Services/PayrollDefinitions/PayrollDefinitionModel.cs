using System.ComponentModel.DataAnnotations;

namespace CDFStaffManagement.Services.PayrollDefinitions
{
    public class PayrollDefinitionModel
    {
        [Key]
        [Required]
        public string[]? Code { get; set; }
        
        public string[]? Description { get; set; }
        [Required]
        public string[]? Type { get; set; }
        [Required]
        public decimal []? Value { get; set; }
        [Required]
        public string[]? OccurenceCode { get; set; }
        [Required]
        public string? Category { get; set; }
        [Required]
        public string? EmployeeCode { get; set; }
    }
}
