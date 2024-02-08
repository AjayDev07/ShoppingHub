using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ShoppingMVC.Data;
using ShoppingMVC.Models;
using ShoppingMVC.Models.ViewModels;

namespace ShoppingMVC.Controllers
{

    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        #region SignUp
        public IActionResult SignUp()
        {
            var signUpViewModel = new SignUpViewModel();
            return View(signUpViewModel);
        }

        [HttpPost]
        public IActionResult SignUp(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if the username is already taken
                if (_context.Users.Any(u => u.UserName == model.UserName))
                {
                    ModelState.AddModelError("UserName", "Username is already taken. Please choose another.");
                    return View(model);
                }

                if (_context.Users.Any(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Email is already taken. Please choose another");
                    return View(model);
                }

                if (model.Password != model.ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "The password and confirm password do not match.");
                    return View(model);
                }

                if (model.PhoneNumber.Length != 10)
                {
                    ModelState.AddModelError("PhoneNumber", "Phone number must be exactly 10 digits.");
                    return View(model);
                }

                // Create a new User entity and populate it with SignUp data
                var newUser = new Login
                {
                    UserName = model.UserName,
                    Password = model.Password,
                    Email = model.Email,
                    Gender = model.Gender,
                    PhoneNumber = model.PhoneNumber,
                    DateOfBirth = model.DateOfBirth,
                    Role = "User" // Set the default role to "User"
                };

                _context.Users.Add(newUser);
                _context.SaveChanges();

                return RedirectToAction("Index", "Account");
            }

            // If the model state is not valid, return to the view with validation errors
            return View(model);
        }
        #endregion

        #region Login
        public IActionResult Index()
        {
            var loginModel = new LoginViewModel();
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
                    var userName = user.UserName;

                    HttpContext.Session.SetInt32("UserId", userId);
                    TempData["UserName"] = userName;

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
        #endregion

        #region Logout
        public async Task<IActionResult> Logout()
        {
            // Clear the user-related information from the session or any other storage mechanism
            HttpContext.Session.Clear();

            // Sign out the user if you are using authentication
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect to the login page or any other desired page
            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}