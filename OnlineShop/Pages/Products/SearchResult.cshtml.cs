using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Services;

namespace OnlineShop.Pages.Products
{
    public class SearchResultModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly OnlineShopDbContext _context;

        public SearchResultModel(IProductService productService, ICategoryService categoryService, OnlineShopDbContext context)
        {
            _productService = productService;
            _categoryService = categoryService;
            _context = context;
        }
        public string Query { get; set; }
        public IList<Category> Categories { get; set; } = new List<Category>();
        public IList<Product> Products { get; set; } = new List<Product>();
        public int? SelectedCategoryId { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }

        public async Task OnGetAsync(string? query, int? categoryId, int pageNumber = 1, int pageSize = 5)
        {
            Query = query;
            SelectedCategoryId = categoryId;

            Categories = (await _categoryService.GetAllCategoriesAsync()).ToList();

            var searchResults = await _productService.SearchProductsAsync(Query, SelectedCategoryId);

            var (products, totalCount) = await _productService.GetPaginatedProductsAsync(pageNumber, pageSize, searchResults);

            foreach (var product in products)
            {
                var averageRating = await _context.Reviews
                    .Where(r => r.ProductId == product.ProductId)
                    .AverageAsync(r => (double?)r.Rating) ?? 0; // Default to 0 if no reviews

                // Store the average rating on each product
                product.AverageRating = Math.Round(averageRating, 2);
            }


            Products = products.ToList();
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }

    }
}
