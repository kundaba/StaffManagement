using System;

namespace CDFStaffManagement.Model.EntityModels
{
    public class Entity
    {
        public int EntityCode { get; set; }
        public string? DisplayName { get; set; }
        public string? FirstName { get; set; }
        public string? SecondName { get; set; }
        public string? LastName { get; set; }
        public string? MaidenName { get; set; }
        public int? TitleId { get; set; }
        public DateTime BirthDate { get; set; }
        public string? Gender { get; set; }
        public string? Nationality { get; set; }
        public int? MaritalStatusId { get; set; }
        public string? Idnumber { get; set; }
        public int? IdnumberType { get; set; }
        public int? EmployeeStatusId { get; set; }
        public int? SocialSecurityNumber { get; set; }
        public string? WorkNumber { get; set; }
        public string? CellNumber { get; set; }
        public string? EmailAddress { get; set; }
        public string? WorkAddress { get; set; }
        public string? PhysicalAddress { get; set; }
        public int? CountryOfBirthId { get; set; }
        public int? BankId { get; set; }
        public int? BankBranchId { get; set; }
        public string? AccountName { get; set; }
        public string? AccountNumber { get; set; }
        public string? CreatedBy { get; set; }
        
        public DateTime? DateCreated { get; set; }
        public DateTime? LastChanged { get; set; }

        public virtual CountryNames? CountryOfBirth { get; set; }
        public virtual EmployeeStatus? EmployeeStatus { get; set; }
        public virtual IdnumberType? IdnumberTypeNavigation { get; set; }
        public virtual MaritalStatus? MaritalStatus { get; set; }
        public virtual TitleDescription? Title { get; set; }
        public virtual Employee? Employee { get; set; }
    }
}
