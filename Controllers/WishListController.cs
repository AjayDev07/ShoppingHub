using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingMVC.Data;
using ShoppingMVC.Models;

namespace ShoppingMVC.Controllers
{
    public class WishListController : Controller
    {
        private readonly ApplicationDbContext _context;
        public WishListController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var wishListItems = _context.Wishlist.Where(w => w.UserId == userId.ToString()).ToList();

            return View(wishListItems);
        }

        [HttpPost]
        public async Task<IActionResult> AddToWishlist([FromBody] Wishlist wishlistItem)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the user ID from session or claim
                var userId = HttpContext.Session.GetInt32("UserId");

                var existingWishlistItem = _context.Wishlist.FirstOrDefault(w => w.UserId == userId.ToString() && w.ItemId == wishlistItem.ItemId);
                if (userId != null)
                {
                    if (existingWishlistItem != null)
                    {
                        // Item with the same UserId and ItemId already exists, update the quantity
                        existingWishlistItem.ItemQuantity += wishlistItem.ItemQuantity;
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        // Item does not exist, add it to the cart
                        wishlistItem.UserId = userId.ToString();
                        _context.Wishlist.Add(wishlistItem);
                        await _context.SaveChangesAsync();
                    }
                    return Json(new { success = true });
                }
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromWishlist(int itemId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var wishlistItem =await _context.Wishlist.FirstOrDefaultAsync(w => w.UserId == userId.ToString() && w.ItemId == itemId);

            if (wishlistItem != null)
            {
                _context.Wishlist.Remove(wishlistItem);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "WishList");
        }
    }
}
