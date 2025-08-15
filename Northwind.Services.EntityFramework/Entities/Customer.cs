// Customer.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Northwind.Services.Repositories;

namespace Northwind.Services.EntityFramework.Entities;
public class Customer
{
    [Key]
    [MaxLength(5)]
    public string CustomerId { get; set; } = null!;

    [Required]
    public string CompanyName { get; set; } = null!;

    public string? ContactName { get; set; }
    public string? ContactTitle { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Region { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }
    public string? Phone { get; set; }
    public string? Fax { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();
}
