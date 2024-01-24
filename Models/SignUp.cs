using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ShoppingMVC.Models
{
    public class SignUp
    {
        [Key]
        public int SignUpId { get; set; }

        
        [Display(Name = "Username")]
        public string UserName { get; set; }

        
        [Display(Name = "Password")]
        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

       
        [Display(Name = "Email")]
        public string Email { get; set; }

        
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Display(Name ="Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name ="Date of Birth")]
        public string DateOfBirth { get; set; }
    }
}
