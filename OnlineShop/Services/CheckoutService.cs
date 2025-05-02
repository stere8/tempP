using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Services
{
    public class CheckoutService
    {
        private readonly OnlineShopDbContext _context;

        public CheckoutService(OnlineShopDbContext context)
        {
            _context = context;
        }

        public async Task<Order> ProcessCheckout(string userId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Retrieve the cart and its items
                var cart = await _context.Carts
                    .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                if (cart == null || !cart.CartItems.Any())
                    throw new Exception("Cart is empty.");

                // Check if the user already has an unprocessed order
                var existingOrder = await _context.Orders
                    .AnyAsync(o => o.UserId == userId && o.Status == "Pending");

                if (existingOrder)
                    throw new Exception("You already have a pending order.");

                // Create the order
                var order = new Order
                {
                    UserId = userId,
                    OrderDate = DateTime.UtcNow,
                    Status = "Pending",
                    TotalAmount = cart.CartItems.Sum(ci => ci.Quantity * ci.Product.Price),
                    OrderItems = cart.CartItems.Select(ci => new OrderItem
                    {
                        ProductId = ci.ProductId,
                        Quantity = ci.Quantity,
                        Price = ci.Product.Price
                    }).ToList()
                };

                _context.Orders.Add(order);

                // Deduct product stock
                foreach (var cartItem in cart.CartItems)
                {
                    var product = await _context.Products.FindAsync(cartItem.ProductId);
                    if (product != null)
                    {
                        if (product.StockQuantity < cartItem.Quantity)
                            throw new Exception($"Not enough stock for {product.Name}.");

                        product.StockQuantity -= cartItem.Quantity;
                    }
                }

                // Remove cart items after checkout
                _context.CartItems.RemoveRange(cart.CartItems);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return order;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
