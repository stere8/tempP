// Shipper.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Northwind.Services.Repositories;

namespace Northwind.Services.EntityFramework.Entities;
public class Shipper
{
    [Key]
    public int ShipperId { get; set; }

    [Required]
    public string CompanyName { get; set; } = null!;

    public string? Phone { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();
}
