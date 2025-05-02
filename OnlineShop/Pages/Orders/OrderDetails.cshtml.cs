using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Pages.Orders
{
    public class OrderDetailsModel : PageModel
    {
        private readonly OnlineShopDbContext _context;
        public bool editable;

        public OrderDetailsModel(OnlineShopDbContext context)
        {
            _context = context;
        }

        public Order Order { get; set; }

        public async Task<IActionResult> OnGetAsync(int orderId)
        {
            // Retrieve the order with related user, items, and products
            Order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (!string.IsNullOrEmpty(Order.UserId))
            {
                Order.User = _context.Users.First(u => u.Id == Order.UserId);
            }

            if (Order == null)
            {
                return NotFound();
            }

            editable = Order.OrderItems.Any();

            return Page();
        }

        // Handle cancel order request for both admin and users
        public async Task<IActionResult> OnPostCancelOrderAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }

            // Only allow the order to be cancelled if it is still "Pending"
            if (order.Status == "Pending")
            {
                order.Status = "Cancelled";
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Orders/OrderList");
        }

        // Handle accept order request (Admin only)
        public async Task<IActionResult> OnPostAcceptOrderAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }

            // Only allow the order to be accepted if it is still "Pending"
            if (order.Status == "Pending")
            {
                order.Status = "Completed"; // Change the status to "Completed" when accepted
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Orders/OrderList");
        }

        // Handle setting the order to pending (Admin only)
        public async Task<IActionResult> OnPostSetOrderToPendingAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }

            // Only allow the order to be set to "Pending" if it is not already
            if (order.Status != "Pending")
            {
                order.Status = "Pending"; // Change the status to "Pending"
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Orders/OrderList");
        }
    }
}
