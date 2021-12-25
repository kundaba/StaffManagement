using System;

namespace CDFStaffManagement.Services.PayslipDetails.Dtos
{
    public class EarningsAndDeductionsDto
    {
        public string? PayrollDefinitionCode { get; set; }
        public string? Description { get; set; }
        public decimal Value { get; set; }
        
        public decimal Amount { get; set; }
        public string? Type { get; set; }
        public DateTime? DateModified { get; set; }
        public string? UserId { get; set; }
        public int PayrollDefinitionFlag { get; set; }
    }
}