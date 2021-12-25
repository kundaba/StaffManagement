using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CDFStaffManagement.Model.EntityModels
{
    public class LeaveDetail
    {
        public int Id {get; set;}
        public int EmployeeId {get; set;}
        public DateTime LeaveAccrualStartDate {get; set;}
        [Column(TypeName = "decimal(18,2)")]
        public decimal LeaveBalance {get; set;}
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MonetaryEquivalent {get; set;}
        
        public int LeaveTypeId {get; set;}
    }
}