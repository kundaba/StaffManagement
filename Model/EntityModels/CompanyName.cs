using System;

namespace CDFStaffManagement.Model.EntityModels
{
    public  class Company
    {
        public int CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public DateTime? DateCreated { get; set; }
    }
}
