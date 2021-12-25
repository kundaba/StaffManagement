using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CDFStaffManagement.Model.EntityModels
{
    public sealed class PayslipDefinition
    {
        
        [Key]
        public int PayslipDefId { get; set; }
        public int EmployeeId { get; set; }
        public string? PayrollDefinitionCode { get; set; }
        
        public string? Description { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal Value { get; set; }
        public string? Type { get; set; }
        public string? OccurenceCode { get; set; }
        public DateTime PeriodStartDate { get; set; }
        public DateTime PeriodEndDate { get; set; }
        public int PayPeriod { get; set; }
        public DateTime? DateModified { get; set; }
        public string? UserId { get; set; }
        public int PayrollDefinitionFlag { get; set; }

        public Employee? Employee { get; set; }

    }
}
