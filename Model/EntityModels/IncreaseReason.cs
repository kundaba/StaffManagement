using System;

namespace CDFStaffManagement.Model.EntityModels
{
    public class IncreaseReason
    {
        public int IncreaseReasonId { get; set; }
        public string? Code { get; set; }
        public string? ShortDescription { get; set; }
        public string? LongDescription { get; set; }
        public string? Status { get; set; }
        public string? UserId { get; set; }
        public DateTime? LastChanged { get; set; }
    }
}
