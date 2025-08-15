using Microsoft.EntityFrameworkCore;

namespace Northwind.Services.EntityFramework.Entities;

public class NorthwindContext : DbContext
{
    public NorthwindContext(DbContextOptions options)
        : base(options)
    {
    }

    public DbSet<Order> Orders { get; set; } = default!;
    public DbSet<Customer> Customers { get; set; } = default!;
    public DbSet<Employee> Employees { get; set; } = default!;
    public DbSet<Shipper> Shippers { get; set; } = default!;
    public DbSet<Product> Products { get; set; } = default!;
    public DbSet<Category> Categories { get; set; } = default!;
    public DbSet<Supplier> Suppliers { get; set; } = default!;
    public DbSet<OrderDetail> OrderDetails { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure relationships and constraints
        base.OnModelCreating(modelBuilder);
    }
}
