// OrderDetail.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.Services.EntityFramework.Entities;
public class OrderDetail
{
    // Composite key (OrderId + ProductId) will be configured via Fluent API in DbContext
    public int OrderId { get; set; }
    public int ProductId { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; } = 1;

    public float Discount { get; set; } = 0;

    public virtual Order Order { get; set; } = null!;
    public virtual Product Product { get; set; } = null!;
}
