using System.Collections.Generic;

namespace CDFStaffManagement.Model.EntityModels
{
    public class UserMenu
    {
        public UserMenu()
        {
            UserMenuMapping = new HashSet<UserMenuMapping>();
        }

        public int Id { get; set; }
        public string MenuDescription { get; set; } = null!;

        public virtual ICollection<UserMenuMapping> UserMenuMapping { get; set; }
    }
}
