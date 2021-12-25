using System;

namespace CDFStaffManagement.Model.EntityModels
{
    public class ErrorLog
    {
        public int Id { get; set; }
        public string? ErrorDescription { get; set; }
        public DateTime? DateLogged { get; set; }
        public string? UserId { get; set; }
    }
}
