using System.Collections.Generic;

namespace CDFStaffManagement.Model.EntityModels
{
    public class CountryNames
    {
        public CountryNames()
        {
            Entity = new HashSet<Entity>();
        }

        public int Id { get; set; }
        public string? CountryCode { get; set; }
        public string? CountryName { get; set; }
        
        public int CountryPhoneCode { get; set; }
        public string? Status { get; set; }

        public virtual ICollection<Entity> Entity { get; set; }
    }
}
