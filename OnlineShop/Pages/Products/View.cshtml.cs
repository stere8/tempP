using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Services;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace OnlineShop.Pages.Products
{
    public class ViewModel : PageModel
    {
        private readonly OnlineShopDbContext _context;
        private readonly CartService _cart;

        public ViewModel(OnlineShopDbContext context, CartService cart)
        {
            _context = context;
            _cart = cart;
        }

        public Product Product { get; set; }
        public List<Product> RelatedProducts { get; set; }
        public List<Review> Reviews { get; set; } // To hold the product's reviews
        public bool CanSubmitReview { get; set; } // Check if the user can submit a review
        public bool IsProductInWishlist { get; set; }

        [BindProperty]
        public int? LowStockThreshold { get; set; }
        public LowStockConfig ExistingLowStockConfig { get; set; }


        public async Task<IActionResult> OnGetAsync(int id)
        {
            ExistingLowStockConfig = await _context.LowStockConfigs.FirstOrDefaultAsync(l => l.ProductId == id);
            LowStockThreshold = ExistingLowStockConfig?.MinThreshold;

            // Fetch product and related products
            Product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (Product == null)
            {
                return NotFound();
            }

            RelatedProducts = await _context.Products
                .Where(p => p.CategoryId == Product.CategoryId && p.ProductId != id)
                .Take(4)
                .ToListAsync();

            // Check if the user has purchased the product (for review submission)
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var hasOrderedProduct = await _context.Orders
                .Where(o => o.UserId == userId)
                .AnyAsync(o => o.OrderItems.Any(oi => oi.ProductId == id));

            CanSubmitReview = hasOrderedProduct;

            var usersWishlist =await _context.Wishlists.FirstOrDefaultAsync(w => w.UserId == userId);

            // Check if the product is in the user's wishlist
            if(usersWishlist != null)
            {
                IsProductInWishlist = await _context.WishlistItems
                       .AnyAsync(wi => wi.ProductId == id && wi.WishlistId == usersWishlist.WishlistId);
            }


            // Fetch reviews with related entities
            Reviews = await _context.Reviews
                .Where(r => r.ProductId == id)
                .OrderByDescending(r => r.CreatedDate)
                .Take(5) // Limit to 5 recent reviews
                .Include(r => r.User) // Include User details
                .Include(r => r.Order) // Include Order details
                .Include(r => r.Product) // Include Product details
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostSetThresholdAsync(int productId, int lowStockThreshold)
        {
            var config = await _context.LowStockConfigs.FirstOrDefaultAsync(c => c.ProductId == productId);
            if (config != null)
            {
                config.MinThreshold = lowStockThreshold;
            }
            else
            {
                config = new LowStockConfig
                {
                    ProductId = productId,
                    MinThreshold = lowStockThreshold
                };
                _context.LowStockConfigs.Add(config);
            }

            await _context.SaveChangesAsync();
            return RedirectToPage(new { id = productId });
        }



        public async Task<IActionResult> OnPostAddToWishlistAsync(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Ensure the user is logged in
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Account/Login");
            }

            var usersWishlist = await _context.Wishlists.FirstOrDefaultAsync(w => w.UserId == userId);

            if (usersWishlist == null) 
            {
                usersWishlist = new Wishlist { UserId = userId };
                _context.Wishlists.Add(usersWishlist);
                await _context.SaveChangesAsync();
            }

            // Check if the product already exists in the user's wishlist
            var existingWishlistItem = await _context.WishlistItems
                .FirstOrDefaultAsync(wi => wi.ProductId == productId && wi.WishlistId == usersWishlist.WishlistId);

            if (existingWishlistItem != null)
            {
                return RedirectToPage(); // Reload the page
            }

            // Add the product to the wishlist
            var wishlistItem = new WishlistItem
            {
                ProductId = productId,
                WishlistId = usersWishlist.WishlistId
            };

            _context.WishlistItems.Add(wishlistItem);
            await _context.SaveChangesAsync();

            return RedirectToPage(); // Reload the page to update button state
        }

        public async Task<IActionResult> OnPostRemoveFromWishlistAsync(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Ensure the user is logged in
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Account/Login");
            }

            // Find the user's wishlist
            var wishlist = await _context.Wishlists
                .FirstOrDefaultAsync(w => w.UserId == userId);

            if (wishlist == null)
            {
                return RedirectToPage(); // If no wishlist found, simply reload
            }

            // Find the wishlist item to remove
            var wishlistItem = await _context.WishlistItems
                .FirstOrDefaultAsync(wi => wi.ProductId == productId && wi.WishlistId == wishlist.WishlistId);

            if (wishlistItem != null)
            {
                _context.WishlistItems.Remove(wishlistItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage(); // Reload the page to update the button state
        }

        public async Task<IActionResult> OnPostAddToCartWithQuantityAsync(int productId, int quantity)
        {
            if (quantity < 1) quantity = 1;

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Account/Login");
            }

            var product = await _context.Products.FindAsync(productId);
            if (product == null || product.StockQuantity < quantity)
            {
                return RedirectToPage("/Cart", new { message = "Product unavailable or insufficient stock" });
            }

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = await _cart.CreateCartAsync(userId);
                _context.Carts.Add(cart);
            }

            if (cart.CartItems == null)
            {
                cart.CartItems = new List<CartItem>();
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (cartItem == null)
            {
                cartItem = new CartItem { ProductId = productId, Quantity = quantity };
                cart.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("");
        }

        // Handle submitting a review
        public async Task<IActionResult> OnPostSubmitReviewAsync(int productId, int rating, string comment)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = await _context.Orders
                .Where(o => o.UserId == userId)
                .FirstOrDefaultAsync(o => o.OrderItems.Any(oi => oi.ProductId == productId));

            if (order == null)
                return BadRequest("You must have ordered this product to leave a review.");

            var review = new Review
            {
                ProductId = productId,
                UserId = userId,
                Rating = rating,
                Comment = comment,
                OrderId = order.OrderId,
                CreatedDate = DateTime.UtcNow
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return RedirectToPage(new { id = productId }); // Reload the page to show the new review
        }

    }
}
