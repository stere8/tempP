using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Services;

namespace OnlineShop.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class DashboardModel : PageModel
    {
        private readonly WishlistService _wishlistService;
        private readonly OnlineShopDbContext _context;

        public DashboardModel(OnlineShopDbContext context, WishlistService wishlistService)
        {
            _context = context;
            _wishlistService = wishlistService;
        }

        public int TotalUsers { get; set; }
        public int TotalOrders { get; set; }
        public int TotalProducts { get; set; }
        public decimal TotalRevenue { get; set; }
        public Dictionary<string, int> OrdersByStatus { get; set; } = new();
        public List<Product> LowStockProducts { get; set; } = new();

        public List<(Product Product, int TotalSold)> TopSellingProducts { get; set; } = new();
        public List<(Product Product, int Count)> TopWishlistedProducts { get; set; } = new(); 
        public List<(Product Product, decimal TotalRevenue)> TopRevenueProducts { get; set; } = new();
        public List<(Category Category, int OrderCount)> MostOrderedCategories { get; set; } = new();
        public List<Order> RecentOrders { get; set; } = new();

        public async Task OnGetAsync()
        {
            TotalUsers = await _context.Users.CountAsync();
            TotalProducts = await _context.Products.CountAsync();
            TotalOrders = await _context.Orders.CountAsync();
            TotalRevenue = await _context.Orders.SumAsync(o => o.TotalAmount);

            OrdersByStatus = await _context.Orders
                .GroupBy(o => o.Status)
                .Select(g => new { g.Key, Count = g.Count() })
                .ToDictionaryAsync(g => g.Key, g => g.Count);

            LowStockProducts = await _context.Products
                .Where(p =>
                    _context.LowStockConfigs.Any(c => c.ProductId == p.ProductId && p.StockQuantity < c.MinThreshold))
                .ToListAsync();

            var topSelling = await _context.OrderItems
                .GroupBy(oi => oi.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    TotalSold = g.Sum(oi => oi.Quantity)
                })
                .OrderByDescending(x => x.TotalSold)
                .Take(5)
                .ToListAsync();

            TopSellingProducts = topSelling
                .Join(_context.Products,
                    ts => ts.ProductId,
                    p => p.ProductId,
                    (ts, p) => (p, ts.TotalSold))
            .ToList();


            TopWishlistedProducts = await _wishlistService.GetTopWishlistedProductsAsync(10);

            var topRevenue = await _context.OrderItems
                .GroupBy(oi => oi.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    TotalRevenue = g.Sum(oi => oi.Quantity * (decimal)oi.Price)
                })
                .OrderByDescending(x => x.TotalRevenue)
                .Take(5)
                .ToListAsync();

            TopRevenueProducts = topRevenue
                .Join(_context.Products,
                    tr => tr.ProductId,
                    p => p.ProductId,
                    (tr, p) => (p, tr.TotalRevenue))
                .ToList();

            var orderedCategories = await _context.OrderItems
                .Include(oi => oi.Product)
                .ThenInclude(p => p.Category)
                .GroupBy(oi => oi.Product.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    OrderCount = g.Count()
                })
                .OrderByDescending(x => x.OrderCount)
                .Take(5)
                .ToListAsync();

            MostOrderedCategories = orderedCategories
                .Select(x => (x.Category, x.OrderCount))
                .ToList();

            RecentOrders = await _context.Orders
                .Include(o => o.User)
                .OrderByDescending(o => o.OrderDate)
                .Take(5)
                .ToListAsync();
        }
    }
}
