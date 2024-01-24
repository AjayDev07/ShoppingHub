using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ShoppingMVC.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
