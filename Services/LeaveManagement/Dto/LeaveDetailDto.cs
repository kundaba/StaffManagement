using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CDFStaffManagement.Services.LeaveManagement.Dto
{
    public class LeaveDetailDto
    {
        public int EmployeeId { get; set; }
        public DateTime LeaveAccrualStartDate { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal LeaveBalance { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MonetaryEquivalent { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal BasicPay { get; set; }
        public int LeaveTypeId { get; set; }
        public static string? ActionType { get; set; }

    }
}