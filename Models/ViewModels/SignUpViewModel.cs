using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ShoppingMVC.Models.ViewModels
{
    public class SignUpViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required (ErrorMessage = "Please enter a Password with atleast 6 characters")]
        [Display(Name = "Password")]
        [PasswordPropertyText]
        [DataType(DataType.Password)]
        [RegularExpression(@"(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,16}")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [PasswordPropertyText]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password is not matching")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage ="Please enter your Email")]
        [Display(Name = "Email")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage ="Invalid Email Address")]
        public string Email { get; set; }

        
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        
        [Display(Name = "Phone Number")]
        [RegularExpression(@"^\d{10}$", ErrorMessage ="Phone number must be exactly 10 digits")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Date of Birth")]
        public string DateOfBirth { get; set; }
    }
}