using OnlineShop.Models;

namespace OnlineShop.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();

        Task<IQueryable<Product>> GetAllProductsQueryable();
        Task<Product> GetProductByIdAsync(int id);
        Task<IQueryable<Product>> SearchProductsAsync(string query, int? categoryId);
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
        Task<(IEnumerable<Product> Products, int TotalCount)> GetPaginatedProductsAsync(int pageNumber, int pageSize,IQueryable<Product> products);
    }
}
