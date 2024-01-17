using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ShoppingMVC.Data;
using ShoppingMVC.Models;

namespace ShoppingMVC.Controllers
{

    public class ShoppingController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ShoppingController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var loginModel = new Login();
            return View(loginModel);
        }

        [HttpPost]
        public IActionResult Index(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check user credentials against the Users table
                var user = _context.Users.FirstOrDefault(u => u.UserName == model.UserName && u.Password == model.Password);

                if (user != null)
                {
                    // Authentication successful
                    var userId = user.Id;
                    var userRole = user.Role;

                    if (userRole != null && userRole == "Admin")
                    {
                        HttpContext.Session.SetInt32("UserId", userId);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        HttpContext.Session.SetInt32("UserId", userId);
                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View(model);
        }







        public async Task<IActionResult> Logout()
        {
            // Clear the user-related information from the session or any other storage mechanism
            HttpContext.Session.Clear();

            // Sign out the user if you are using authentication
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect to the login page or any other desired page
            return RedirectToAction("Index", "Home");
        }
    }
}