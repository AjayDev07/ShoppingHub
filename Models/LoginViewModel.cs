using System.ComponentModel.DataAnnotations;

namespace ShoppingMVC.Models
{
    public class LoginViewModel
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }


    }
}
