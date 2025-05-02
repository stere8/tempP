using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using System.Security.Claims;

namespace OnlineShop.Pages.Orders
{
    public class OrderListModel : PageModel
    {
        private readonly OnlineShopDbContext _context;

        public OrderListModel(OnlineShopDbContext context)
        {
            _context = context;
        }

        public IList<Order> Orders { get; set; }

        public async Task OnGetAsync()
        {
            var userType = User.FindFirstValue(ClaimTypes.Role);

            if (userType == "Admin") 
            {
                Orders = await _context.Orders
               .Include(o => o.User)
               .Include(o => o.OrderItems)
               .ThenInclude(oi => oi.Product)
               .ToListAsync();  // Admin can see all orders
            }

            else
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                Orders = await _context.Orders
                    .Where(o => o.UserId == userId)
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .ToListAsync();  // Regular users only see their own orders
            }

            foreach(Order order in Orders)
            {
                if (!string.IsNullOrEmpty(order.UserId))
                {
                    order.User = _context.Users.First(u => u.Id == order.UserId);
                }
            }            

            // Get all orders from the database, including the related OrderItems and Products

        }
    }
}
