using System;
using System.Collections.Generic;

namespace CDFStaffManagement.Model.EntityModels
{
    public sealed class Department
    {
        public Department()
        {
            Employee = new HashSet<Employee>();
        }

        public int DepartmentId { get; set; }
        public string? DepartmentCode { get; set; }
        public string? LongDescription { get; set; }
        public DateTime? LastChanged { get; set; }
        public string? ChangedByUser { get; set; }
        
        public string? Status { get; set; }

        public ICollection<Employee> Employee { get; set; }
    }
}
