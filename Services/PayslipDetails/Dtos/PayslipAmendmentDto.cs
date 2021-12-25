using System.ComponentModel.DataAnnotations;

namespace CDFStaffManagement.Services.PayslipDetails.Dtos
{
    public class PayslipAmendmentDto
    {
        [Required]
        public string? EmployeeCode { get; set; }
        [Required]
        public string? Code { get; set; }
        [Required]
        public decimal AmendedValue { get; set; }
        [Required]
        public string? PayrollDefinitionType { get; set; }
    }
}