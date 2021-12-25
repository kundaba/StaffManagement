using System;

namespace CDFStaffManagement.Model.EntityModels
{
    public class DisciplinaryCases
    {
        public int CaseId { get; set; }
        public int EmployeeId { get; set; }
        public string? EmployeeCode { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? CaseType { get; set; }
        public string? CaseDescription { get; set; }
        public string? Category { get; set; }
        public DateTime? DateOffenceCommitted { get; set; }
        public string? CaseOutcome { get; set; }
        public DateTime? DateCreated { get; set; }
        public string? CreatedBy { get; set; }
    }
}