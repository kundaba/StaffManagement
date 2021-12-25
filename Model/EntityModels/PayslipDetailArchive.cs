using System;

namespace CDFStaffManagement.Model.EntityModels
{
    public class PayslipDetailArchive
    {
        public int PayslipArchiveId { get; set; }
        public int? EmployeeId { get; set; }
        public int? DeductionDefId { get; set; }
        public decimal? DeductionAmount { get; set; }
        public int? EarningDefId { get; set; }
        public decimal? EarningAmount { get; set; }
        public DateTime? PayPeriod { get; set; }
        public DateTime? ExportDate { get; set; }
        public int? ExportedByUserId { get; set; }
    }
}
