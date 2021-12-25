using System.Collections.Generic;

namespace CDFStaffManagement.Model.EntityModels
{
    public class NatureOfContract
    {
        public NatureOfContract()
        {
            Employee = new HashSet<Employee>();
        }

        public int Id { get; set; }
        public string? ContractTypeCode { get; set; }
        public string? ContractTypeDecsription { get; set; }
        public string? Status { get; set; }

        public virtual ICollection<Employee> Employee { get; set; }
    }
}
