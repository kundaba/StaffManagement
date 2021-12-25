using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CDFStaffManagement.Services.Employee.Dto
{
    public class QualificationSubmissionDto
    {
        [Required]
        public string? DocumentType { get; set; }
        [Required]
        public IFormFile[]? Document { get; set; }
        public string[]? QualificationType { get; set; }
        public string[]? FieldOfStudy { get; set; }
        [Required]
        public DateTime[]? StartDate { get; set; }
        public DateTime[]? EndDate { get; set; }
        
        [Required]
        public string? EmployeeCode  { get; set; }
        [Required] public string ActionType { get; set; } = "Update";
    }
}