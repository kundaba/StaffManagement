using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CDFStaffManagement.Services.Employee.Dto
{
    public class NewEmployee
    {
        [Key]
        [Required]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z0-9''-'\s]{1,10}$", ErrorMessage = "Special characters are not allowed")]
        public string? EmployeeCode { get; set; } 
        [Required]
        [DataType(DataType.Date)]
        public DateTime? DateEngaged { get; set; }
        public DateTime? LeaveStartDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public int? TerminationReasonId { get; set; }
        [Required]
        public int? JobTitleId { get; set; }
        [Required]
        public int? JobGradeId { get; set; }
        [Required]
        public decimal BasicPay { get; set; }
        [Required]
        public int? JobGeneralId { get; set; }
        public int? EmployeeStatusId { get; set; }
        public string? PositionCode { get; set; }
        [Required]
        public int? NatureOfContractId { get; set; }
        [Required]
        public int? DepartmentId { get; set; }
        public int? UserId { get; set; }
        public string? DisplayName { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Special characters are not allowed")]
        public string? FirstName { get; set; }
        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Special characters are not allowed")]
        public string? SecondName { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Special characters are not allowed")]
        public string? LastName { get; set; }
        [DataType(DataType.Text)]
        public string? MaidenName { get; set; }
        public int? TitleId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        [Required]
        public string? Gender { get; set; }
        public int Nationality { get; set; }
        [Required]
        public int? MaritalStatusId { get; set; }
        [Required]
        public string? IdNumber { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9''-'\s]{1,40}$", ErrorMessage = "Special characters are not allowed")]
        public int? IdNumberType { get; set; }
        [RegularExpression(@"^[0-9''-'\s]{1,40}$", ErrorMessage = "Special characters are not allowed")]
        public int? SocialSecurityNumber { get; set; }
        public string? WorkNumber { get; set; }
        [Required]
        public string? CellNumber { get; set; }
        [DataType(DataType.EmailAddress)]
        public string? EmailAddress { get; set; }
        public string? WorkAddress { get; set; }
        [Required]
        public string? PhysicalAddress { get; set; }
        [Required]
        public int? BankId { get; set; }
        [Required]
        public int? BankBranchId { get; set; }
        [Required]
        public string? AccountName { get; set; }
        [Required]
        [RegularExpression(@"^[0-9''-'\s]{1,40}$", ErrorMessage = "Only numerical characters allowed")]
        public string? AccountNumber { get; set; }
        [Required]
        public IEnumerable<long>? FileIds { get; set; }
    }
}
