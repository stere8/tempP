using OnlineShop.Models;
using System.Threading.Tasks;

namespace OnlineShop.Services
{
    public interface ICartService
    {
        Task<Cart> GetCartByUserAsync(string userId);
        Task<Order> ConvertCartToOrder(Cart cart, string userId);
        Task ClearCartAsync(int cartId);
        Task<ICollection<CartItem>> GetCartItemsAsync(string userId);
    }
}