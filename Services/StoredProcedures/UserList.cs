using System;
using System.ComponentModel.DataAnnotations;

namespace CDFStaffManagement.Services.StoredProcedures
{
    public class UserList
    {
        [Key]
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? EmailAddress { get; set; }
        public string? RoleDescription { get; set; }
        public string? ProfileStatus { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastLogon { get; set; }
    }
}
