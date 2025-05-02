using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Services;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

public class IndexModel : PageModel
{
    private readonly OnlineShopDbContext _context;
    private readonly ICategoryService _categoryService;

    public IndexModel(OnlineShopDbContext context, ICategoryService categoryService)
    {
        _context = context;
        _categoryService = categoryService;
    }

    public IList<Category> Categories { get; set; }
    public int CartItemCount { get; set; }

    // Admin alert counts
    public int LowStockCount { get; set; }
    public int PendingOrdersCount { get; set; }

    public async Task OnGetAsync()
    {
        // CART COUNT FOR AUTHENTICATED USERS
        if (User.Identity.IsAuthenticated)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            CartItemCount = cart?.CartItems?.Sum(ci => ci.Quantity) ?? 0;
            ViewData["CartItemCount"] = CartItemCount;
        }

        // LOAD CATEGORIES
        var allCategories = await _categoryService.GetAllCategoriesAsync();
        Categories = allCategories
            .OrderBy(c => c.CategoryId == 8 ? 1 : 0)
            .ThenBy(c => c.Name)
            .ToList();

        // ONLY show admin alerts if user is Admin
        if (User.IsInRole("Admin"))
        {
            // EXAMPLE: Low stock = any product with StockQuantity < 5
            // Adjust logic for your threshold or a LowStockConfig table
            LowStockCount = await _context.Products
                .CountAsync(p => p.StockQuantity < 5);

            // EXAMPLE: Count orders with status == "Pending"
            PendingOrdersCount = await _context.Orders
                .CountAsync(o => o.Status == "Pending");
        }
    }
}
