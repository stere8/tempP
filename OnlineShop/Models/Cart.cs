using OnlineShop.Data;

namespace OnlineShop.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public string UserId { get; set; }  // Foreign key to ApplicationUser
        public ApplicationUser User { get; set; }  // Navigation property
        public CartStatus Status { get; set; }
        public ICollection<CartItem> CartItems { get; set; }  // Navigation property
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}