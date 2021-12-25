using System;

namespace CDFStaffManagement.Model.EntityModels
{
    public sealed class Employee
    {
        public int EmployeeId { get; set; }
        public int EntityId { get; set; }
        public string? EmployeeCode { get; set; }
        public DateTime? DateEngaged { get; set; }
        public DateTime? LeaveStartDate { get; set; }
        
        public DateTime? PensionStartDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public int? TerminationReasonId { get; set; }
        public int? JobTitleId { get; set; }
        public int? JobGradeId { get; set; }
        public int? JobGeneralId { get; set; }
        public int? EmployeeStatusId { get; set; }
        public int? ReportToEmployeeId { get; set; }
        public int? NatureOfContractId { get; set; }
        public int? DepartmentId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public Department? Department { get; set; }
        public EmployeeStatus? EmployeeStatus { get; set; }
        public Entity? Entity { get; set; }
        public JobGeneral? JobGeneral { get; set; }
        public JobGrade? JobGrade { get; set; }
        public JobTitle? JobTitle { get; set; }
        public NatureOfContract? NatureOfContract { get; set; }
    }
}
