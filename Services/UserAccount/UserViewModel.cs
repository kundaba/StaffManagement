
using System.ComponentModel.DataAnnotations;

namespace CDFStaffManagement.Services.UserAccount
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="This Field is required")]
        [Display(Name = "Username")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }
    }
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "Provide employee code")]
        public string? EmployeeCode { get; set; }

        [Required(ErrorMessage = "Provide username")]
        [Display(Name = "Username")]
        [RegularExpression(@"^[a-zA-Z0-9''-'\s]{1,40}$", ErrorMessage = "Special characters are not allowed")]
        [StringLength(100, ErrorMessage = "The username must be at least {2} characters long.", MinimumLength = 4)]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Provide email address")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Select user role")]
        public int UserRole { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }

        //[DataType(DataType.Password)]
        //[Display(Name = "Confirm password")]
        //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        //public string ConfirmPassword { get; set; }
    }
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "Username")]
        [RegularExpression(@"^[a-zA-Z0-9''-'\s]{1,40}$", ErrorMessage = "Special characters are not allowed")]
        [StringLength(100, ErrorMessage = "The username must be at least {2} characters long.", MinimumLength = 2)]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }
    }
    public class ChangePassword
    {
        [Required]
        [Display(Name = "Username")]
        [RegularExpression(@"^[a-zA-Z0-9''-'\s]{1,40}$", ErrorMessage = "Special characters are not allowed")]
        [StringLength(100, ErrorMessage = "The username must be at least {2} characters long.", MinimumLength = 2)]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
    }
    
    public class EditUserDetail
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        [EmailAddress]
        public string? EmailAddress { get; set; }
        [Required]
        public int UserRole { get; set; }
    }
}
