using System.Collections.Generic;

namespace CDFStaffManagement.Model.EntityModels
{
    public class JobGeneral
    {
        public JobGeneral()
        {
            Employee = new HashSet<Employee>();
        }

        public int JobGeneralId { get; set; }
        public string? LongDescription { get; set; }
        public string? Status { get; set; }

        public virtual ICollection<Employee> Employee { get; set; }
    }
}
