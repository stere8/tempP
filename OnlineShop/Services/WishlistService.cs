using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Services
{
    public class WishlistService
    {
        private readonly OnlineShopDbContext _context;

        public WishlistService(OnlineShopDbContext context)
        {
            _context = context;
        }

        public async Task<Wishlist> GetWishlistByUserAsync(string userId)
        {
            return await _context.Wishlists
                .Include(w => w.WishlistItems)
                .ThenInclude(wi => wi.Product)
                .FirstOrDefaultAsync(w => w.UserId == userId);
        }

        public async Task AddItemToWishlistAsync(string userId, int productId)
        {
            var wishlist = await GetWishlistByUserAsync(userId);

            if (wishlist == null)
            {
                wishlist = new Wishlist
                {
                    UserId = userId,
                    CreatedDate = DateTime.UtcNow
                };

                _context.Wishlists.Add(wishlist);
                await _context.SaveChangesAsync();
            }

            if (wishlist.WishlistItems.Any(wi => wi.ProductId == productId))
            {
                return; // Item already in the wishlist
            }

            var wishlistItem = new WishlistItem
            {
                ProductId = productId,
                WishlistId = wishlist.WishlistId
            };

            _context.WishlistItems.Add(wishlistItem);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveItemFromWishlistAsync(int wishlistItemId)
        {
            var wishlistItem = await _context.WishlistItems.FindAsync(wishlistItemId);
            if (wishlistItem != null)
            {
                _context.WishlistItems.Remove(wishlistItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<(Product Product, int Count)>> GetTopWishlistedProductsAsync(int topN)
        {
            // Group by ProductId in the WishlistItems table
            var grouping = await _context.WishlistItems
                .GroupBy(wi => wi.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .Take(topN)
                .ToListAsync();

            // Join with Products table to get product details
            var result = grouping
                .Join(_context.Products,
                      grouped => grouped.ProductId,
                      product => product.ProductId,
                      (grouped, product) => (product, grouped.Count))
                .ToList();

            return result;
        }

    }
}
