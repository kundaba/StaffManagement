using System;

namespace CDFStaffManagement.Model.EntityModels
{
    public class EmployeeHistory
    {
        public int EmployeeHistoryId { get; set; }
        public int EmployeeId { get; set; }
        public string? EmployeeCode { get; set; }
        public int EmployeeStatusId { get; set; }
        public DateTime? TerminationDate { get; set; }
        public int? TerminationReasonId { get; set; }
        public DateTime? DateEngaged { get; set; }
        public int? JobGradeId { get; set; }
        public int? JobTitleId { get; set; }
        public string? PositionCode { get; set; }
        public int? JobGeneralId { get; set; }
        public int? NatureOfContractId { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
