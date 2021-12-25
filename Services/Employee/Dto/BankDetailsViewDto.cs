using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CDFStaffManagement.Services.Employee.Dto
{
    public class BankDetailsViewDto
    {
        [Required]
        public string? EmployeeCode { get; set; }
        public string? BankName { get; set; }
  
        public string? BranchName { get; set; }
        public string? BranchCode { get; set; }
        [Required]
        public string? AccountNumber { get; set; }
        [Required]
        public string? AccountName { get; set; }
        public string? StatusCode { get; set; }
        public int IsDefaultBank { get; set; }
        public DateTime? DateCreated { get; set; }
    }
}
