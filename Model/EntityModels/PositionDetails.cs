using System;

namespace CDFStaffManagement.Model.EntityModels
{
    public  class PositionDetails
    {
        public int PositionCodeId { get; set; }
        
        public string? JobTitleCode { get; set; }
        
        public string? PositionCode { get; set; }
        
        public string? ShortDescription { get; set; }
        
        public string? LongDescription { get; set; }
        
        public int DepartmentId { get; set; }
        
        public string? ReportsToPositionCode { get; set; }
        
        public string? EmployeeCode { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime? VacancyDate { get; set; }
        
        public string? Status { get; set; }
        
        public string? CreatedBy { get; set; }
        
        public DateTime CreatedDate { get; set; }
    }
}