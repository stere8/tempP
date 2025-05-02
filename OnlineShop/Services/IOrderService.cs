using OnlineShop.Models;

namespace OnlineShop.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderFromCartAsync(string userId);
    }
}
