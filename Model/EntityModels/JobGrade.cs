using System.Collections.Generic;

namespace CDFStaffManagement.Model.EntityModels
{
    public sealed class JobGrade
    {
        public JobGrade()
        {
            Employee = new HashSet<Employee>();
        }

        public int JobGradeId { get; set; }
        public string JobGradeCode { get; set; } = null!;
        public string JobGradeDescription { get; set; } = null!;
        public string? Status { get; set; }

        public ICollection<Employee>? Employee { get; set; }
    }
}
