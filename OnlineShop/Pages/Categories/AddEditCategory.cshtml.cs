using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineShop.Models;
using OnlineShop.Services;

namespace OnlineShop.Pages.Categories
{
    [Authorize(Roles = "Admin")]
    public class AddEditCategoryModel : PageModel
    {
        private readonly ICategoryService _categoryService;

        [BindProperty]
        public Category Category { get; set; }
        public int? CategoryId { get; set; }
        [BindProperty]
        public string currentCategoryUrl { get; set; }

        public AddEditCategoryModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> OnGetAsync(int? CategoryId)
        {
            this.CategoryId = CategoryId;

            if (CategoryId.HasValue)
            {
                Category = await _categoryService.GetCategoryByIdAsync(CategoryId.Value);
                currentCategoryUrl = Category.ImageUrl;

                if (Category == null)
                {
                    return NotFound();
                }
            }
            else
            {
                Category = new Category();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile ImageFile)
        {
            ModelState.Remove(nameof(currentCategoryUrl));
            ModelState.Remove(nameof(ImageFile));

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Category.CategoryId == 0) // Adding a new category
            {
                if (ImageFile != null)
                {
                    Category.ImageUrl = await SaveImageAsync(ImageFile);
                }
                await _categoryService.AddCategoryAsync(Category);
            }
            else // Editing an existing category
            {
                if (ImageFile != null)
                {
                    Category.ImageUrl = await SaveImageAsync(ImageFile);
                }
                else
                {
                    Category.ImageUrl = currentCategoryUrl; // Retain existing image if no new image is provided
                }

                await _categoryService.UpdateCategoryAsync(Category);
            }

            return RedirectToPage("./Index");
        }


        public async Task<IActionResult> OnPostRemoveImageAsync()
        {
            if (Category.CategoryId == 0)
            {
                return BadRequest("Cannot remove image for a new category.");
            }

            // Get the current category
            var category = await _categoryService.GetCategoryByIdAsync(Category.CategoryId);
            if (category == null)
            {
                return NotFound();
            }

            // Remove the current image
            if (!string.IsNullOrEmpty(category.ImageUrl))
            {
                await DeleteImage(category.ImageUrl);
                category.ImageUrl = null; // Clear the image URL
                await _categoryService.UpdateCategoryAsync(category);
            }

            return RedirectToPage(new { CategoryId = category.CategoryId });
        }


        public async Task<IActionResult> OnPostDeleteAsync()
        {
            if (Category.CategoryId != 0)
            {
                await _categoryService.DeleteCategoryAsync(Category.CategoryId);
                DeleteImage(currentCategoryUrl);
            }
            return RedirectToPage("./Index");
        }

        private async Task<string> GetUploadsFolder()
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            Directory.CreateDirectory(uploadsFolder);
            return uploadsFolder;
        }

        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            var uploadsFolder = GetUploadsFolder().Result;

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return uniqueFileName;
        }

        private async Task DeleteImage(string filename)
        {
            var uploadsFolder = GetUploadsFolder().Result;
            var filePath = Path.Combine(uploadsFolder, filename);
            System.IO.File.Delete(filePath);
        }
    }
}