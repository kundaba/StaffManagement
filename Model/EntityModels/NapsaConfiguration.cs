using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CDFStaffManagement.Model.EntityModels
{
    public class NapsaConfiguration
    {
        public int Id { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal Percentage { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal MaximumCeiling { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ? EndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
    } 
}