using System;

namespace CDFStaffManagement.Model.EntityModels
{
    public class PromotionHistory
    {
        public int Id { get; set; }
        public string? EmployeeCode { get; set; }
        public string? OldPositionCode { get; set; }
        public string? NewPositionCode { get; set; }
        public DateTime PromotionDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
    }
}