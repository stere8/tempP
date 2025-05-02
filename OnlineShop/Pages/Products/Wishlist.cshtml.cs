using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Services;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineShop.Pages.Products
{
    public class WishlistModel : PageModel
    {
        private readonly WishlistService _wishlistService;
        private readonly OnlineShopDbContext _context;

        public WishlistModel(WishlistService wishlistService, OnlineShopDbContext context)
        {
            _wishlistService = wishlistService;
            _context = context;
        }

        public IList<Product> Products { get; set; }
        public ICollection<WishlistItem> WishlistItems { get; set; }
        public ICollection<Wishlist> AdminWishlists { get; set; }
        public List<(Product Product, int Count)> TopWishlistedProducts { get; set; } = new();

        public bool IsAdmin { get; set; } // Check if the user is an Admin


        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            IsAdmin = User.IsInRole("Admin");

            WishlistItems = new List<WishlistItem>();

            if (IsAdmin)
            {
                var allWishlists = await _context.Wishlists
                   .Include(w => w.WishlistItems)
                   .ThenInclude(wi => wi.Product)
                   .ToListAsync();

                Products = await _context.Products.ToListAsync();

                foreach (var wishlist in allWishlists)
                {
                    wishlist.WishlistItems = _context.WishlistItems.Where(wi => wi.WishlistId == wishlist.WishlistId).ToList();
                    wishlist.User = _context.Users.FirstOrDefault(u => u.Id == wishlist.UserId);
                }
                AdminWishlists = allWishlists;
            }
            else
            {
                if (user == null)
                {
                    return RedirectToPage("/Account/Login");
                }

                var wishlist = await _wishlistService.GetWishlistByUserAsync(userId);

                if (wishlist == null)
                {
                    // Handle case where wishlist is not found (maybe create one or redirect to an error page)
                    return RedirectToPage("/Error");
                }

                WishlistItems = wishlist.WishlistItems;
                Products = await _context.Products.ToListAsync();
            }

            TopWishlistedProducts = await _wishlistService.GetTopWishlistedProductsAsync(15);

            Products = await _context.Products.ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAddToWishlistAsync(int productId)
        {
            var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Account/Login");
            }

            // Add the product to the wishlist
            await _wishlistService.AddItemToWishlistAsync(userId, productId);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRemoveFromWishlistAsync(int wishlistItemId)
        {
            // Remove the wishlist item
            await _wishlistService.RemoveItemFromWishlistAsync(wishlistItemId);
            return RedirectToPage();
        }
    }
}
