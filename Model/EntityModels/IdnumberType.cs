using System.Collections.Generic;

namespace CDFStaffManagement.Model.EntityModels
{
    public class IdnumberType
    {
        public IdnumberType()
        {
            Entity = new HashSet<Entity>();
        }

        public int IdnumberTypeId { get; set; }
        public string? Idcode { get; set; }
        public string? LongDescription { get; set; }
        public string? Status { get; set; }

        public virtual ICollection<Entity> Entity { get; set; }
    }
}
