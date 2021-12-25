using System.Collections.Generic;

namespace CDFStaffManagement.Model.EntityModels
{
    public class UserStatus
    {
        public UserStatus()
        {
            UserDetail = new HashSet<UserDetail>();
        }

        public int StatusId { get; set; }
        public string? StatusDescription { get; set; }

        public virtual ICollection<UserDetail> UserDetail { get; set; }
    }
}
