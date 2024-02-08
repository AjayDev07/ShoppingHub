using ShoppingMVC.Models.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace ShoppingMVC.Models
{
    public class Items
    {
        [Key]
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string? ItemDiscription { get; set; }
        public int ItemQuantity { get; set; }
        public int ItemPrice { get; set; }
    }
}