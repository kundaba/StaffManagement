using System;
using System.Collections.Generic;
namespace CDFStaffManagement.Model.EntityModels
{
    public class Bank
    {
        public Bank()
        {
            BankBranch = new HashSet<BankBranch>();
        }

        public int BankId { get; set; }
        public string Code { get; set; } = null!;
        public string BankName { get; set; } = null!;
        public string? Status { get; set; }
        public DateTime? LastChanged { get; set; }

        public virtual ICollection<BankBranch> BankBranch { get; set; }
    }
}
