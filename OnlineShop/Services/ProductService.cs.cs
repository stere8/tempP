using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Services
{
    public class ProductService : IProductService
    {
        private readonly OnlineShopDbContext _context;

        public ProductService(OnlineShopDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products.Include(p => p.Category).ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task<IQueryable<Product>> SearchProductsAsync(string? query, int? categoryId)
        {
            var products = _context.Products.Include(p => p.Category).AsQueryable();

            // Apply search query if provided
            if (!string.IsNullOrWhiteSpace(query))
            {
                products = products.Where(p => p.Name.Contains(query) || p.Description.Contains(query));
            }

            // Filter by category if provided
            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value);
            }

            return products;
        }

        public async Task AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Product>> GetProductbyCategoryIdAsync(int categoryId)
        {
            return await _context.Products.Where(p => p.CategoryId == categoryId).ToListAsync();
        }

        public async Task<(IEnumerable<Product> Products, int TotalCount)> GetPaginatedProductsAsync(int pageNumber, int pageSize, IQueryable<Product> products)
        {

            int totalCount = await products.CountAsync();

            var paginatedProducts = await products
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (paginatedProducts, totalCount);
        }

        public async Task<IQueryable<Product>> GetAllProductsQueryable()
        {
            var products = _context.Products.Include(p => p.Category).AsQueryable();

            return products;
        }
    }
}
