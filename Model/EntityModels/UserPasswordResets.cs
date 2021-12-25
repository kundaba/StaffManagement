using System;

namespace CDFStaffManagement.Model.EntityModels
{
    public class UserPasswordResets
    {
        public int ResetId { get; set; }
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string ? ResetToken { get; set; }
        public DateTime? ResetDate { get; set; }
        public int? TokenStatusId { get; set; }

        public virtual UserDetail? User { get; set; }
    }
}
