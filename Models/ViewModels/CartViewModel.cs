namespace ShoppingMVC.Models.ViewModels
{
    public class CartViewModel
    {
        public List<Cart> CartItems { get; set; }
        public int TotalItemsInCart { get; set; }
    }
}