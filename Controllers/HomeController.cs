using Microsoft.AspNetCore.Mvc;
using ShoppingMVC.Data;
using ShoppingMVC.Models;
using ShoppingMVC.Models.ViewModels;
using System.Diagnostics;

namespace ShoppingMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context; 

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context) 
        {
            _logger = logger;
            _context = context; // Initialize the context
        }

        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            //var userName = HttpContext.Session.GetString("UserName");
            var user = _context.Users.Find(userId);
            
            if (user != null && user.Role == "Admin")
            {
                ViewData["Admin"] = true;
                // Fetch a list of items from the database  
            }
            else
            {
                ViewData["Admin"] = false;
            }
            var items = _context.Items.ToList();
            // Pass the list of items to the view
            return View(items);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult AddItemIndex(Items newItem)
        {
            return View("AddItemIndex");
        }

        [HttpPost]
        public IActionResult AddItem(Items newItem)
        {
            if (ModelState.IsValid)
            {
                _context.Items.Add(newItem);
                _context.SaveChanges();

                // Redirect to the Index action to refresh the list of items
                return RedirectToAction("Index", "Home");
            }
            // If the model state is not valid, return to the view with validation errors
            return View("AddItemIndex");
        }


        //public IActionResult FilterByCategory(string category)
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

        //    //Fetch items based on the category
        //    var items = string.IsNullOrEmpty(category) ? _context.Items.ToList() : _context.Items.Where(i => i.ItemCategory == category).ToList();

        //    //Pass the list of items and category to the view
        //    ViewBag.SelectedCategory = category;
        //    return View("Index", items);

        //}
    }
}