using System.ComponentModel.DataAnnotations;

namespace ShoppingMVC.Models
{
    public class Cart
    {
        [Key]
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int ItemPrice { get; set; }
        public int ItemQuantity { get; set; }
        public string UserId { get; set; }
    }
}