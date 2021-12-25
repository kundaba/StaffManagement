using System;

namespace CDFStaffManagement.Model.EntityModels
{
    public sealed class BankBranch
    {
        public int BranchId { get; set; }
        public int BankId { get; set; }
        public string? BranchCode { get; set; }
        public string? BranchName { get; set; }
        public string? Status { get; set; }
        public DateTime? LastChanged { get; set; }

        public Bank? Bank { get; set; }
    }
}
