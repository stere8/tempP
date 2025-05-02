using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Services
{
    public class LowStockService
    {
        private readonly OnlineShopDbContext _context;

        public LowStockService(OnlineShopDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetLowStockProductsAsync()
        {
            return await _context.Products
                .Where(p => _context.LowStockConfigs
                    .Any(cfg => cfg.ProductId == p.ProductId && p.StockQuantity <= cfg.MinThreshold))
                .ToListAsync();
        }
    }
}
