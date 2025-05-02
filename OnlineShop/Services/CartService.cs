using OnlineShop.Data;
using OnlineShop.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Services
{
    public class CartService : ICartService
    {
        private readonly OnlineShopDbContext _context;

        public CartService(OnlineShopDbContext context)
        {
            _context = context;
        }

        // Get the cart for the user
        public async Task<Cart> GetCartByUserAsync(string userId)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        // Convert cart to an order
        public async Task<Order> ConvertCartToOrder(Cart cart, string userId)
        {
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                Status = "Pending",
                OrderItems = cart.CartItems.Select(ci => new OrderItem
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    Price = ci.Product.Price
                }).ToList(),
                TotalAmount = cart.CartItems.Sum(ci => ci.Product.Price * ci.Quantity)
            };

            // Set the cart status as paid
            cart.Status = CartStatus.Paid;

            // Clear cart after checkout
            await ClearCartAsync(cart.CartId);

            return order;
        }

        // Create a new cart for the user if not exists
        public async Task<Cart> CreateCartAsync(string userId)
        {
            var cart = new Cart
            {
                UserId = userId,
                Status = CartStatus.Active, // Default status when cart is created
                CreatedDate = DateTime.UtcNow
            };

            _context.Carts.Add(cart);
            await _context.SaveChangesAsync(); // This will save and auto-generate the CartId

            return cart;
        }

        // Process checkout
        public async Task<Order> ProcessCheckout(string userId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var cart = await _context.Carts
                    .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                if (cart == null || !cart.CartItems.Any())
                    throw new Exception("Cart is empty.");

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

        public async Task<int> GetActiveOrdersCountAsync()
        {
            return await _context.Orders
                .Where(o => o.Status == "Pending")  // Adjust status as needed
                .CountAsync();
        }


        // Clear cart after order
        public async Task ClearCartAsync(int cartId)
        {
            var cartItems = await _context.CartItems
                .Where(ci => ci.CartId == cartId)
                .ToListAsync();

            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }

        // Get all items in the user's cart
        public async Task<ICollection<CartItem>> GetCartItemsAsync(string userId)
        {
            var cart = await GetCartByUserAsync(userId);
            return cart?.CartItems ?? new List<CartItem>();
        }
    }
}

