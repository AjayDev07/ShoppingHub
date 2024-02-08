namespace ShoppingMVC.Models.ViewModels
{
    public class ItemsIndexViewModel
    {
        public IEnumerable<Items> Items { get; set;}
        public IEnumerable<string> ?Categories { get; set;}
    }
}
