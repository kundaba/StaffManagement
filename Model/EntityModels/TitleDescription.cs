using System.Collections.Generic;

namespace CDFStaffManagement.Model.EntityModels
{
    public class TitleDescription
    {
        public TitleDescription()
        {
            Entity = new HashSet<Entity>();
        }

        public int TitleId { get; set; }
        public string? TitleDescription1 { get; set; }

        public virtual ICollection<Entity> Entity { get; set; }
    }
}
