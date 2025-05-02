using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Services;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly OnlineShopDbContext _context;

        public IndexModel(OnlineShopDbContext context, IProductService productService)
        {
            _context = context;
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }

        public IList<Product> Products { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }

        public async Task OnGetAsync(int pageNumber = 1, int pageSize = 75)
        {
            // Get all products
            var allProductsQuery = await _productService.GetAllProductsQueryable();

            // Paginate products
            var (paginatedProducts, totalCount) = await _productService.GetPaginatedProductsAsync(pageNumber, pageSize, allProductsQuery);
            Products = paginatedProducts.ToList();

            // Calculate total pages
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            // Get average rating for each product in a single query
            var averageRatings = await _context.Reviews
                .GroupBy(r => r.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    AverageRating = g.Average(r => (double?)r.Rating) ?? 0
                })
                .ToListAsync();


            // Assign the average ratings to the respective products
            foreach (var product in Products)
            {
                var productRating = averageRatings.FirstOrDefault(r => r.ProductId == product.ProductId);
                product.AverageRating = Math.Round(productRating?.AverageRating ?? 0,2); // Default to 0 if no reviews
            }
        }
    }
}
