using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ShoppingMVC.Data;
using ShoppingMVC.Models;
using ShoppingMVC.Models.ViewModels;

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
            ViewBag.IsLoggedIn = HttpContext.Session.GetInt32("UserId") != null;
            return View(loginModel);
        }

        [HttpPost]
        public IActionResult Index(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.UserName == model.UserName && u.Password == model.Password);

                if (user != null)
                {
                    // Authentication successful
                    var userId = user.Id;
                    var userRole = user.Role;

                    HttpContext.Session.SetInt32("UserId", userId);

                    if (userRole != null && userRole == "Admin")
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    // Invalid username or password
                    ViewData["ErrorMessage"] = "Invalid username or password";
                    return View();
                }
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