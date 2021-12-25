using System.Collections.Generic;

namespace CDFStaffManagement.Model.EntityModels
{
    public class MaritalStatus
    {
        public MaritalStatus()
        {
            Entity = new HashSet<Entity>();
        }

        public int MaritalStatusId { get; set; }
        public string? LongDescription { get; set; }
        public string? Status { get; set; }

        public virtual ICollection<Entity> Entity { get; set; }
    }
}
