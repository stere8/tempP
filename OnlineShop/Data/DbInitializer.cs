using Microsoft.AspNetCore.Identity;
using OnlineShop.Models;

namespace OnlineShop.Data
{
    public class DbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Ensure roles exist
            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            if (!await roleManager.RoleExistsAsync("User"))
                await roleManager.CreateAsync(new IdentityRole("User"));

            // Add admin user
            if (await userManager.FindByEmailAsync("admin@example.com") == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    FirstName = "Admin",
                    LastName = "User",
                    Address = "123 Admin St."
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // Add a sample normal user
            if (await userManager.FindByEmailAsync("user@example.com") == null)
            {
                var normalUser = new ApplicationUser
                {
                    UserName = "user@example.com",
                    Email = "user@example.com",
                    FirstName = "Normal",
                    LastName = "User",
                    Address = "456 User St."
                };

                var result = await userManager.CreateAsync(normalUser, "User123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(normalUser, "User");
                }
            }


            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<OnlineShopDbContext>();

            // Ensure category exists
            if (!context.Categories.Any())
            {
                context.Categories.Add(new Category
                {
                    Name = "Default Category",
                    Summary = "Auto-seeded category"
                });
                await context.SaveChangesAsync();
            }

            // Ensure product exists
            if (!context.Products.Any())
            {
                var category = context.Categories.First();

                context.Products.Add(new Product
                {
                    Name = "Test Product",
                    Description = "Temporary test item for checkout flow",
                    Price = 19.99m,
                    StockQuantity = 10,
                    CategoryId = category.CategoryId,
                    ImageUrl = "/images/test-product.jpg"
                });

                await context.SaveChangesAsync();
            }


        }


    }
}
