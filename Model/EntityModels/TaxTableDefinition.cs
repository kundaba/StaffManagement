using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CDFStaffManagement.Model.EntityModels
{
    public class TaxTableDefinition
    {
        public int Id { get; set; }
        public string? BandDescription { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? LowerLimit { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? UperLimit { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Amount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Percentage { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? DateCreated { get; set; }
        public string? Status { get; set; }
        public string? CreatedBy { get; set; }
    }
}
