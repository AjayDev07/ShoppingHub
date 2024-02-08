using System.ComponentModel.DataAnnotations;

namespace ShoppingMVC.Models
{
    public class Category
    {
        [Key]
        public  int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool IsActive { get; set; }
    }
}