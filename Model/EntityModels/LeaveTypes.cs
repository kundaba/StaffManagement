using System;

namespace CDFStaffManagement.Model.EntityModels
{
    public class LeaveTypes
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? LeaveTypeDescription { get; set; }
        public int Entitlement { get; set; }
        public string? ApplicableGender { get; set; }
        public string? BalanceBroughtForwardOption { get; set; }
        public string? Cycle { get; set; }
        public DateTime DateCreated { get; set; }
        public string? CreatedBy { get; set; }
    }
}