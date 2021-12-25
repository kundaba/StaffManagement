using System;
using System.Collections.Generic;

namespace CDFStaffManagement.Model.EntityModels
{
    public sealed class JobTitle
    {
        public JobTitle()
        {
            Employee = new HashSet<Employee>();
        }

        public int JobTitleId { get; set; }
        public string? Jobcode { get; set; }
        public string? ShortDescription { get; set; }
        public string? LongDescription { get; set; }
        
        public int? JobGradeId { get; set; }
        
        public string? Status { get; set; }
        public DateTime? LastChanged { get; set; }
        
        public string? ChangedByUser {get;set;}

        public ICollection<Employee> Employee { get; set; }
    }
}
