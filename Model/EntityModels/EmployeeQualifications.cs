using System;

namespace CDFStaffManagement.Model.EntityModels
{
    public class EmployeeQualifications
    { 
        public int FileId { get; set; }
        public string GuId { get; set; }
        public int? EmployeeId { get; set; }
        public string? DocumentType { get; set; }
        public string? QualificationType { get; set; }
        public string? FieldOfStudy { get; set; }
        public byte[]? DocumentContent { get; set; }
        public string? DocumentName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}