using System;

namespace CDFStaffManagement.Model.EntityModels
{
    public class UserAuditLogs
    {
        public int AuditId { get; set; }
        public string? Guid { get; set; }
        public string? UserName { get; set; }
        public int EmployeeId { get; set; }
        public DateTime? ActionDate { get; set; }
        public string? ActionType { get; set; }
        public string? Action { get; set; }
        public string? FieldName { get; set; }
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
    }
}
