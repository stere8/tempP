using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace OnlineShop.Pages.Products
{
    public class ProductReviewsModel : PageModel
    {
        private readonly OnlineShopDbContext _context;

        public ProductReviewsModel(OnlineShopDbContext context)
        {
            _context = context;
        }

        public List<Review> Reviews { get; set; }

        public async Task<IActionResult> OnGetAsync(int productId)
        {
            // Get the current user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Check if the current user is an admin
            if (User.IsInRole("Admin"))
            {
                // Fetch all reviews for the product if the user is an admin
                Reviews = await _context.Reviews
                    .OrderByDescending(r => r.CreatedDate)
                    .Include(r => r.User)
                    .ToListAsync();
            }
            else
            {
                // Fetch only the current user's reviews for the product if they are not an admin
                Reviews = await _context.Reviews
                    .Where(r => r.UserId == userId)
                    .OrderByDescending(r => r.CreatedDate)
                    .Include(r => r.User)
                    .ToListAsync();
            }

            return Page();
        }
    }
}
