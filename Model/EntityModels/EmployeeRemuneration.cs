using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CDFStaffManagement.Model.EntityModels
{
    public class EmployeeRemuneration
    {
        public int RemunerationId { get; set; }
        public int EmployeeId { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal RemunerationAmount { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }
        
        public string? Reason { get; set; }
        
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public string? UserId { get; set; }
    }
}
