using System.ComponentModel.DataAnnotations;

namespace MVCApp.Models
{
    public class EmployeeModel
    {

        [Display(Name = "Employee ID")]
        // Range of number characters min, max, and error message
        [Range(100000, 999999, ErrorMessage = "You need to enter a valid EmployeeId")]
        public int EmployeeId { get; set; }

       
        // This is not needed this is an example for custom ErrorMessage 
        // [Required(ErrorMessage = "You need to give us your first name.")]
        [Display(Name = "First Name")]
        public required string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public required string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public required string EmailAddress { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Confirm Email")]
        [Compare("EmailAddress", ErrorMessage = "The Email and Confim Email must match.")]
        public required string ConfirmEmail { get; set; }


        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 10, ErrorMessage ="You need to provide a long enough password.")]
        public required string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Your password and confirm password do not match.")]
        public required string ConfirmPassword { get; set; }
        
    }
}
