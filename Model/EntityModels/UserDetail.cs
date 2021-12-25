using System;
using System.Collections.Generic;

namespace CDFStaffManagement.Model.EntityModels
{
    public class UserDetail
    {
        public UserDetail()
        {
            UserPasswordResets = new HashSet<UserPasswordResets>();
        }

        public int UserId { get; set; }
        public int? EmployeId { get; set; }
        public string? Username { get; set; }
        public string? EmailAddress { get; set; }
        public string? Password { get; set; }
        public int? UserRoleId { get; set; }
        public int? ProfileStatus { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? LastLogon { get; set; }
        public int? FailedLoginAttempts { get; set; }

        public virtual UserStatus? ProfileStatusNavigation { get; set; }
        public virtual UserRoles? UserRole { get; set; }
        public virtual ICollection<UserPasswordResets> UserPasswordResets { get; set; }
    }
}
