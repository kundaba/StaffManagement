using System.Collections.Generic;

namespace CDFStaffManagement.Model.EntityModels
{
    public class UserRoles
    {
        public UserRoles()
        {
            UserDetail = new HashSet<UserDetail>();
            UserMenuMapping = new HashSet<UserMenuMapping>();
        }

        public int UserRoleId { get; set; }
        public string RoleDescription { get; set; } = null!;

        public virtual ICollection<UserDetail> UserDetail { get; set; }
        public virtual ICollection<UserMenuMapping> UserMenuMapping { get; set; }
    }
}
