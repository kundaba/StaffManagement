namespace CDFStaffManagement.Model.EntityModels
{
    public class UserMenuMapping
    {
        public int UserMenuMappingId { get; set; }
        public int? UserRoleId { get; set; }
        public int? UserMenuId { get; set; }

        public virtual UserMenu? UserMenu { get; set; }
        public virtual UserRoles? UserRole { get; set; }
    }
}
