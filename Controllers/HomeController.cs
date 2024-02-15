using Microsoft.AspNetCore.Mvc;
using ShoppingMVC.Data;
using ShoppingMVC.Models;
using ShoppingMVC.Models.ViewModels;
using System.Diagnostics;
using static ShoppingMVC.Constants.Discription;

namespace ShoppingMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index(string category, string searchItem = null)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var user = _context.Users.Find(userId);

            if (user != null && user.Role == "Admin")
            {
                ViewData["Admin"] = true;
            }
            else
            {
                ViewData["Admin"] = false;
            }

            // Fetch items based on the selected category or fetch all items if no category is specified
            IQueryable<Items> itemsQuery = string.IsNullOrEmpty(category) ? _context.Items : _context.Items.Where(item => item.ItemDiscription.Contains(category));

            // Filter items based on the search query (item name)
            if (!string.IsNullOrEmpty(searchItem))
            {
                itemsQuery = itemsQuery.Where(item => item.ItemName.Contains(searchItem));
            }

            var items = itemsQuery.ToList();
            var categories = _context.Category.Select(i => i.CategoryName).ToList();

            foreach (var item in items)
            {
                int itemDescriptionValue;
                if (int.TryParse(item.ItemDiscription, out itemDescriptionValue))
                {
                    item.ItemDiscription = ((ItemDiscription)itemDescriptionValue).ToString();
                }
            }

            var model = new ItemsIndexViewModel();
            model.Items = items;
            model.Categories = categories;

            // Pass the list of items to the view
            return View(model);
        }




        //public IActionResult Index(string category, string searchItem = null)
        //{
        //    var userId = HttpContext.Session.GetInt32("UserId");
        //    var user = _context.Users.Find(userId);

        //    if (user != null && user.Role == "Admin")
        //    {
        //        ViewData["Admin"] = true;
        //    }
        //    else
        //    {
        //        ViewData["Admin"] = false;
        //    }

        //    //Fetching the data for Cart Notification
        //    var cartItems = _context.Cart.Where(c => c.UserId == userId.ToString()).ToList();
        //    var totalItems = cartItems.Count();
        //    ViewBag.TotalItemsInCart = totalItems;


        //    //Fetching the data for Wishlist notification
        //    var wishListItems = _context.Wishlist.Where(w => w.UserId == userId.ToString()).ToList();
        //    var wishlisttotalItems = wishListItems.Count();
        //    ViewBag.TotalItemsInWislist = wishlisttotalItems;

        //    // Fetch items based on the selected category or fetch all items if no category is specified
        //    var items = string.IsNullOrEmpty(category) ? _context.Items.ToList() : _context.Items.Where(item => item.ItemDiscription.Contains(category)).ToList();
        //    //var categories = _context.Category.Select(i => i.CategoryName).ToList();
        //    var categories = _context.Category.Select(i => i.CategoryName).ToList();
        //    foreach (var item in items)
        //    {
        //        int itemDescriptionValue;
        //        if (int.TryParse(item.ItemDiscription, out itemDescriptionValue))
        //        {
        //            item.ItemDiscription = ((ItemDiscription)itemDescriptionValue).ToString();
        //        }
        //    }
        //    var model = new ItemsIndexViewModel();
        //    model.Items = items;
        //    model.Categories = categories;

        //    // Pass the list of items to the view
        //    return View(model);
        //}


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        private ShoppingMVC.Constants.Discription.ItemDiscription GetEnumValueFromString(string enumString)
        {
            if (Enum.TryParse<ShoppingMVC.Constants.Discription.ItemDiscription>(enumString, out var enumValue))
            {
                return enumValue;
            }

            // Return the first enum value as a default or handle the case where the conversion fails
            return Enum.GetValues(typeof(ShoppingMVC.Constants.Discription.ItemDiscription)).Cast<ShoppingMVC.Constants.Discription.ItemDiscription>().FirstOrDefault();
        }


        #region AddItem
        public IActionResult AddItemIndex(ItemViewModel newItem)
        {
            // Retrieve item descriptions for dropdown list
            var itemDescriptions = _context.Category.Select(c => c.CategoryName).ToList();

            // Set ViewBag.ItemDescriptions
            ViewBag.ItemDescriptions = itemDescriptions;
            return View("AddItemIndex");
        }

        [HttpPost]
        public IActionResult AddItem(ItemViewModel newItem)
        {
            if (ModelState.IsValid)
            {
                // Map the view model to the actual model
                var itemToAdd = new Items
                {
                    ItemId = newItem.ItemId,
                    ItemName = newItem.ItemName,
                    // Store the enum name in the database
                    ItemDiscription = newItem.ItemDiscription,
                    ItemQuantity = newItem.ItemQuantity,
                    ItemPrice = newItem.ItemPrice
                };

                _context.Items.Add(itemToAdd);
                _context.SaveChanges();

                // Redirect to the Index action to refresh the list of items
                return RedirectToAction("Index", "Home");
            }
            // Return the view with the item descriptions
            return View("AddItemIndex", newItem);
        }
        #endregion


        #region AddItemDescription
        public IActionResult AddItemDescriptionIndex(CategoryViewModel newCategory)
        {
            return View("AddItemDescriptionIndex");
        }

        [HttpPost]
        public IActionResult AddItemDescription(CategoryViewModel newCategory)
        {
            if (ModelState.IsValid)
            {
                var categoryToAdd = new Category
                {
                    CategoryName = newCategory.CategoryName,
                    IsActive = true
                };
                _context.Category.Add(categoryToAdd);
                _context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            // Retrieve all item descriptions
            var allItemDescriptions = _context.Category.Select(i => i.CategoryName).ToList();
            return View("AddItemDescriptionIndex");
        }
        #endregion


        #region EditItem
        [HttpPost]
        public IActionResult EditItem(ItemViewModel model)
        {
            var itemToEdit = _context.Items.Find(model.ItemId);
            if (itemToEdit != null)
            {
                itemToEdit.ItemName = model.ItemName;
                itemToEdit.ItemQuantity = model.ItemQuantity;
                itemToEdit.ItemPrice = model.ItemPrice;

                // Set the ItemDiscription directly from the view model
                //itemToEdit.ItemDiscription = model.ItemDiscription;

                _context.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }
        #endregion


        #region RemoveItem
        [HttpPost]
        public IActionResult RemoveItem(int itemId)
        {
            var itemToRemove = _context.Items.Find(itemId);

            if (itemToRemove != null)
            {
                // Remove the item from the Items table
                _context.Items.Remove(itemToRemove);

                // Remove the item from all user carts
                var cartItemsToRemove = _context.Cart.Where(c => c.ItemId == itemId);
                _context.Cart.RemoveRange(cartItemsToRemove);

                // Remove the item from all the user wishlist
                var wishlistItemsToRemove = _context.Wishlist.Where(w => w.ItemId == itemId);
                _context.Wishlist.RemoveRange(wishlistItemsToRemove);

                _context.SaveChanges();
            }

            return RedirectToAction("Index", "Home");
        } 
        #endregion
    }
}