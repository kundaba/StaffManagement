using System;

namespace CDFStaffManagement.Model.EntityModels
{
    public class LeaveEntitlementView
    {
        public string? EmployeeCode {get; set;}
        public string? LeaveAccrualStartDate {get; set;}
        public string? LeaveTypeDescription {get; set;}
        public int Entitlement {get; set;}
        public decimal LeaveBalance {get; set;}
        public decimal? MonetaryValue {get; set;}
    }
}