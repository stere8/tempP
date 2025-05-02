using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Services
{
    public class ReviewService
    {
        private readonly OnlineShopDbContext _context;

        public ReviewService(OnlineShopDbContext context)
        {
            _context = context;
        }

        public async Task AddReviewAsync(string userId, int productId, string comment, int rating)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.UserId == userId && o.OrderItems.Any(oi => oi.ProductId == productId));

            if (order == null)
            {
                throw new Exception("You must have ordered this product before leaving a review.");
            }

            var review = new Review
            {
                UserId = userId,
                ProductId = productId,
                Comment = comment,
                Rating = rating,
                OrderId = order.OrderId, // Associate the review with the order
                CreatedDate = DateTime.UtcNow
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
        }

        public async Task<IList<Review>> GetReviewsByProductAsync(int productId)
        {
            return await _context.Reviews
                .Where(r => r.ProductId == productId)
                .ToListAsync();
        }

        public async Task<double> GetAverageRatingByProductAsync(int productId)
        {
            return await _context.Reviews
                .Where(r => r.ProductId == productId)
                .AverageAsync(r => r.Rating);
        }
    }
}
