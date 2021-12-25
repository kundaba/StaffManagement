using System.Collections.Generic;
using CDFStaffManagement.Model.EntityModels;

namespace CDFStaffManagement.Services.PayslipDetails.Dtos
{
    public class PayslipDetailsDto
    {
        public EmployeeDetail? EmployeeDetail { get; set; }
        
        public List<PayslipDetail>? PayslipDetails { get; set; }
    }
}