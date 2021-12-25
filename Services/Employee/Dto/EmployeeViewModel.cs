using System;

namespace CDFStaffManagement.Services.Employee.Dto
{
 
    public class Employee
    {
        public int EmployeeId { get; set; }
        public int EntityCode { get; set; }
        public string? EmployeeCode { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MaidenName { get; set; }
        public string? FullName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Gender { get; set; }
        public string? Nationality { get; set; }
        public string? IdNumber { get; set; }
        public string? Title { get; set; }
        public DateTime? DateEngaged { get; set; }
        public string? LeaveStartDate { get; set; }
        public string? TerminationDate { get; set; }
        public string? TerminationReason { get; set; }
        public string? JobTitle { get; set; }
        public string? JobGrade { get; set; }
        public string? JobGeneral { get; set; }
        public int StatusId { get; set; }
        public string? EmployeeStatus { get; set; }
        public string? NatureOfContract { get; set; }
        public string? Department { get; set; }
    }
}
