using System;

namespace CDFStaffManagement.Model.EntityModels
{
    public class PayrollDeductionDef
    {
        public int DefId { get; set; }
        public string? DeductionCode { get; set; }
        public string? DeductionDecsription { get; set; }
        public string? Formula { get; set; }
        public string? Status { get; set; }
        public DateTime? DateCreated { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public string? ModifiedBy { get; set; }
        public int LineFlag { get; set; }
    }

}
