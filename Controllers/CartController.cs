using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingMVC.Data;
using ShoppingMVC.Models;

namespace ShoppingMVC.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Retrieve the user's cart items

            var userId = HttpContext.Session.GetInt32("UserId");
            var cartItems = _context.Cart.Where(c => c.UserId == userId.ToString()).ToList();

            return View(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] Cart cartItem)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the user ID from session or claim
                var userId = HttpContext.Session.GetInt32("UserId");

                var existingCartItem = _context.Cart.FirstOrDefault(c => c.UserId == userId.ToString() && c.ItemId == cartItem.ItemId);
                if (userId != null)
                {
                    if (existingCartItem != null)
                    {
                        // Item with the same UserId and ItemId already exists, update the quantity
                        existingCartItem.ItemQuantity += cartItem.ItemQuantity;
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        // Item does not exist, add it to the cart
                        cartItem.UserId = userId.ToString();
                        _context.Cart.Add(cartItem);
                        await _context.SaveChangesAsync();
                    }
                    return Json(new { success = true });
                }
            }
            return Json(new { success = false });
        }


        
        [HttpPost]
        public async Task<IActionResult> MoveToCart(int itemId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var wishlistItem = await _context.Wishlist.FirstOrDefaultAsync(w => w.UserId == userId.ToString() && w.ItemId == itemId);

            if (wishlistItem != null)
            {
                // Check if the item already exists in the cart
                var existingCartItem = _context.Cart.FirstOrDefault(c => c.UserId == userId.ToString() && c.ItemId == itemId);

                if (existingCartItem != null)
                {
                    // Item with the same userId and itemId already exists in the cart, update the quantity
                    existingCartItem.ItemQuantity += wishlistItem.ItemQuantity;
                }
                else
                {
                    // Item does not exist in the cart, add it
                    var cartItem = new Cart
                    {
                        ItemId = wishlistItem.ItemId,
                        ItemName = wishlistItem.ItemName,
                        ItemPrice = wishlistItem.ItemPrice,
                        ItemQuantity = wishlistItem.ItemQuantity,
                        UserId = wishlistItem.UserId
                    };

                    _context.Cart.Add(cartItem);
                }

                // Remove the item from the wishlist
                _context.Wishlist.Remove(wishlistItem);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "WishList");
        }


        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int itemId)
        {
            var userId=HttpContext.Session.GetInt32("UserId");
            var cartItem = await _context.Cart.FirstOrDefaultAsync(c => c.UserId == userId.ToString() && c.ItemId == itemId);

            if(cartItem!= null)
            {
                _context.Cart.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index","Cart");
        }
    }
}