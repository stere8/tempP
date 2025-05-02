using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace OnlineShop.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string? ImageUrl { get; set; }    // Add this
        public string? Summary { get; set; }      // Add this

        [ValidateNever]  // Prevent validation on Products collection
        public ICollection<Product>? Products { get; set; }
    }
}
