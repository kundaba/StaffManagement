using System.Collections.Generic;

namespace CDFStaffManagement.Model.EntityModels
{
    public class EmployeeStatus
    {
        public EmployeeStatus()
        {
            Employee = new HashSet<Employee>();
            Entity = new HashSet<Entity>();
        }

        public int StatusId { get; set; }
        public string? StatusCode { get; set; }
        public string? StatusDescription { get; set; }

        public virtual ICollection<Employee> Employee { get; set; }
        public virtual ICollection<Entity> Entity { get; set; }
    }
}
