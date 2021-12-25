using System;
using System.ComponentModel.DataAnnotations;

namespace CDFStaffManagement.Model.EntityModels
{
    public class TerminationReason
    {
        [Key]
        public int TerminationReasonId { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public DateTime? DateCreated { get; set; }
    }
}
