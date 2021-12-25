using System;
using System.ComponentModel.DataAnnotations;

namespace CDFStaffManagement.Services.DisciplinaryCases.Dto
{
    public class DisciplinaryCaseDto
    {
        public int CaseId { get; set; }
        [Required]
        public string? EmployeeCode { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [Required]
        public string? CaseType { get; set; }
        [Required]
        public string? CaseDescription { get; set; }
        [Required]
        public string? Category { get; set; }
        [Required]
        public DateTime? DateOffenceCommitted { get; set; }
        [Required]
        public string? CaseOutcome { get; set; }
        public DateTime? DateCreated { get; set; }
        public string? CreatedBy { get; set; }
    }
}