using System;
using Microsoft.AspNetCore.Http;

namespace CDFStaffManagement.Services.Employee.Dto
{
    public class QualificationDocumentsDto
    {
        public string? GuId { get; set; }
        
        public string? DocumentType { get; set; }
        public string? QualificationType { get; set; }
        public string? FieldOfStudy { get; set; }
        public string? DocumentName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? ExpiryStatus { get; set; }
    }
}