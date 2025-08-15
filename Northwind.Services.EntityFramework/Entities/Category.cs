// Category.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Northwind.Services.Repositories;

namespace Northwind.Services.EntityFramework.Entities;
public class Category
{
    [Key]
    public int CategoryId { get; set; }

    [Required]
    public string CategoryName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
}
