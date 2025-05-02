using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineShop.Pages
{
    public class CartModel : PageModel
    {
        private readonly OnlineShopDbContext _context;

        public CartModel(OnlineShopDbContext context)
        {
            _context = context;
        }

        public List<CartItem> CartItems { get; set; } = new();
        public decimal TotalAmount { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return RedirectToPage("/Account/LoginModel");
            }

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                return Page();
            }

            CartItems = cart.CartItems.ToList();
            TotalAmount = CartItems.Sum(ci => ci.Quantity * ci.Product.Price);

            return Page();
        }

        public IActionResult OnPostCheckoutAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Account/Login");
            }

            return RedirectToPage("/Checkout/Index"); //  Go to real checkout
        }


        public async Task<IActionResult> OnPostUpdateQuantityAsync(int productId, int quantity)
        {
            if (quantity < 1) quantity = 1;

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return RedirectToPage("Account/Login"); //  Corrected path

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart != null)
            {
                var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
                if (cartItem != null)
                {
                    var product = await _context.Products.FindAsync(productId);
                    if (product != null && quantity <= product.StockQuantity)
                    {
                        cartItem.Quantity = quantity;
                        await _context.SaveChangesAsync();
                    }
                }
            }

            return RedirectToPage("Cart"); 
        }

        public async Task<IActionResult> OnPostRemoveItemAsync(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return RedirectToPage("Account/Login"); //  Corrected path

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart != null)
            {
                var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
                if (cartItem != null)
                {
                    _context.CartItems.Remove(cartItem);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToPage("Cart");
        }
    }
}
