using Microsoft.AspNetCore.Mvc;
using ShoppingMVC.Data;
using ShoppingMVC.Models;
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
            // Fetch a list of items from the database  
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
    }
}
