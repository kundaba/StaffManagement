using System.ComponentModel.DataAnnotations;

namespace CDFStaffManagement.Services.Employee.Dto
{
    public class EmployeeAmendmentDto
    {
        [Required]
        public string? EmployeeCode { get; set; }
        [Required]
        public string []? FieldName { get; set; }
    
        public string []? CurrentValue { get; set; }
        [Required]
        public string []? NewValue { get; set; }
    }
}