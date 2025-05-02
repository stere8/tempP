using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Services
{
    public class OrderService : IOrderService
    {
        private readonly OnlineShopDbContext _context;

        public OrderService(OnlineShopDbContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateOrderFromCartAsync(string userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || !cart.CartItems.Any())
                throw new InvalidOperationException("Cart is empty or missing");

            decimal totalAmount = 0;
            var orderItems = new List<OrderItem>();

            foreach (var cartItem in cart.CartItems)
            {
                if (cartItem.Product == null || cartItem.Product.StockQuantity < cartItem.Quantity)
                    throw new InvalidOperationException($"Insufficient stock for {cartItem.Product?.Name}");

                // Decrease stock
                cartItem.Product.StockQuantity -= cartItem.Quantity;

                var orderItem = new OrderItem
                {
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Product.Price
                };
                orderItems.Add(orderItem);
                totalAmount += cartItem.Quantity * cartItem.Product.Price;
            }

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                Status = "Pending",
                TotalAmount = totalAmount,
                OrderItems = orderItems
            };

            _context.Orders.Add(order);

            // Remove cart items
            _context.CartItems.RemoveRange(cart.CartItems);
            await _context.SaveChangesAsync();

            return order;
        }
    }
}
