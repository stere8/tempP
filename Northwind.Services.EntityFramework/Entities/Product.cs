// Product.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Northwind.Services.Repositories;

namespace Northwind.Services.EntityFramework.Entities;
public class Product
{
    [Key]
    public int ProductId { get; set; }

    [Required]
    public string ProductName { get; set; } = null!;

    // Foreign keys are nullable because SupplierID/CategoryID can be null in the schema
    public int? SupplierId { get; set; }
    public int? CategoryId { get; set; }

    public string? QuantityPerUnit { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? UnitPrice { get; set; } = 0;

    public int? UnitsInStock { get; set; } = 0;
    public int? UnitsOnOrder { get; set; } = 0;
    public int? ReorderLevel { get; set; } = 0;

    public bool Discontinued { get; set; } = false;

    // Navigation properties
    public virtual Category? Category { get; set; }
    public virtual Supplier? Supplier { get; set; }
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new HashSet<OrderDetail>();
}
