using Microsoft.EntityFrameworkCore;

namespace Northwind.Services.EntityFramework.Entities;

public class NorthwindContext : DbContext
{
    public NorthwindContext(DbContextOptions options)
        : base(options)
    {
    }
}
