using System;
using System.ComponentModel.DataAnnotations;

namespace CDFStaffManagement.Services.Parameters
{
 
    public class Departments
    {
        [Key]
        public int DepartmentId { get; set; }
        [Required]
        public string? DepartmentCode {get;set;}
        [Required]
        public string? LongDescription { get; set; }
        public DateTime? LastChanged { get; set; }
        [Required]
        public string? Status { get; set; }
    }
    public class JobGrades
    {
        [Key]
        public int JobGradeId { get; set; }
        [Required]
        public string? JobGradeCode { get; set; }
        [Required]
        public string? JobGradeDescription { get; set; }
        public string? Status { get; set; }
    }
    public class JobTitles
    {
        [Key]
        public int JobTitleId { get; set; }
        [Required]
        public string? JobCode { get; set; }
        [Required]
        public string? ShortDescription { get; set; }
        [Required]
        public string? LongDescription { get; set; }
        [Required]
        public int? JobGradeId { get; set; }
        public string? JobGrade { get; set; }
        public string? Status { get; set; }
    }
    public class Banks
    {
        [Key]
        public int BankId { get; set; }
        [Required]
        public string? BankCode { get; set; }
        [Required]
        public string? BankName { get; set; }
        public string? Status { get; set; }
        
        public DateTime? LastChanged { get; set; }        
    }
    public class Branch
    {
        [Key]
        public int BranchId { get; set; }
        [Required]
        public int BankId { get; set; }
        [Required]
        public string? BranchCode { get; set; }
        [Required]
        public string? BranchName { get; set; }
        public string? BankName { get; set; }
        public string? Status { get; set; }
        
        public DateTime? LastChanged { get; set; }
    }
    public class PayrollDeductionLine
    {
        [Key]
        public int DefId { get; set; }
        [Required]
        public string? DeductionCode { get; set; }
        [Required]
        public string? DeductionDescription { get; set; }
        
        public string? Formula { get; set; }
        public string? Status { get; set; }
        
        public DateTime? CreatedDate { get; set; }
        
        public string? CreatedBy { get; set; }
    }
    public class PayrollEarningLine
    {
        [Key]
        public int DefId { get; set; }
        [Required]
        public string? EarningLineCode { get; set; }
        [Required]
        public string? EarningLineDescription { get; set; }
        
        public string? Formula { get; set; }
        public string? Status { get; set; }
        
        public DateTime? CreatedDate { get; set; }
        
        public string? CreatedBy { get; set; }
    }
    public class LookupData
    {
        [Key]
        public int? Id { get; set; }
        public string? Description { get; set; }
    }
    public class TaxDefinition
    {
        [Key]
        public int Id { get; set; }
        [RegularExpression(@"^[a-zA-Z ''-'\s]{1,40}$", ErrorMessage = "Special characters are not allowed")]
        public string? BandDescription { get; set; }
        public decimal? LowerLimit { get; set; }
        public decimal? UperLimit { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Percentage { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? DateCreated { get; set; }
        public string? Status { get; set; }
        public string? CreatedBy { get; set; }
    }
    
    public class TerminationReasons
    {
        [Key]
        public int TerminationReasonId { get; set; }
        [Required]
        public string? Code { get; set; }
        [Required]
        public string? Description { get; set; }
        public string? Status { get; set; }
    }
}
