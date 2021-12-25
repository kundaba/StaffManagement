using System;

namespace CDFStaffManagement.Model.EntityModels
{
    public class PayslipDetail
    {
        public int PayslipId { get; set; }
        public int? EmployeeId { get; set; }
        public int? DeductionDefId { get; set; }
        public decimal? DeductionAmount { get; set; }
        public int? EarningDefId { get; set; }
        public decimal? EarningAmount { get; set; }
        public DateTime? PayPeriod { get; set; }
        
        public int LineFlag { get; set; }
        public int? UserId { get; set; }
    }
}
