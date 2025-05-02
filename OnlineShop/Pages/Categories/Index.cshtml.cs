using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineShop.Models;
using OnlineShop.Services;

namespace OnlineShop.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly ICategoryService _categoryService;

        public IndexModel(ICategoryService categoryService)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
        }

        public IList<Category> Categories { get; set; }

        public async Task OnGetAsync()
        {
            var allCategories = await _categoryService.GetAllCategoriesAsync();

            Categories = allCategories
                .OrderBy(c => c.CategoryId == 8 ? 1 : 0) // This puts ID 8 last
                .ThenBy(c => c.Name)
                .ToList();
        }
    }
}