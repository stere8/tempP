using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OnlineShop.Models;
using OnlineShop.Services;

namespace OnlineShop.Pages.Products
{
    [Authorize(Roles = "Admin")]
    public class AddEditProductModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        [BindProperty]
        public Product Product { get; set; }
        public SelectList Categories { get; set; }
        required public int? ProductId { get; set; }
        [BindProperty]
        public string currentProductUrl { get; set; }

        public AddEditProductModel(ICategoryService categoryService, IProductService productService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            ProductId = id;

            var categories = await _categoryService.GetAllCategoriesAsync();
            Categories = new SelectList(categories, "CategoryId", "Name");

            if (id.HasValue)
            {
                Product = await _productService.GetProductByIdAsync(id.Value);
                currentProductUrl = Product.ImageUrl;

                if (Product == null)
                {
                    return NotFound();
                }
            }
            else
            {
                Product = new Product();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile ImageFile)
        { 
            if (Product.ProductId == 0)
            {
                ModelState.Remove(nameof(currentProductUrl));
            }

            if (!ModelState.IsValid && ImageFile != null)
            {
                var categories = await _categoryService.GetAllCategoriesAsync();
                Categories = new SelectList(categories, "CategoryId", "Name");
                return Page();
            }

            if (Product.ProductId == 0)
            {
                if (ImageFile != null)
                {
                    Product.ImageUrl = await SaveImageAsync(ImageFile);
                }
                await _productService.AddProductAsync(Product);
            }
            else
            {
                if (ImageFile != null)
                {
                    Product.ImageUrl = await SaveImageAsync(ImageFile);
                }
                else
                {
                    Product.ImageUrl = currentProductUrl;
                }
                
                await _productService.UpdateProductAsync(Product);
            }

            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            if (Product.ProductId != 0)
            {
                await _productService.DeleteProductAsync(Product.ProductId);
                DeleteImage(currentProductUrl);
            }
            return RedirectToPage("./Index");
        }

        private async Task<string> GetUploadsFolder()
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            Directory.CreateDirectory(uploadsFolder); // Ensure the folder exists
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

            return uniqueFileName; // Return the relative path for display
        }

        private async Task DeleteImage(string filename)
        {
            var uploadsFolder = GetUploadsFolder().Result;
            var filePath = Path.Combine(uploadsFolder, filename);
            System.IO.File.Delete(filePath);
        }
    }
}