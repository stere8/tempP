using OnlineShop.Data;

namespace OnlineShop.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }  // Foreign key to ApplicationUser
        public ApplicationUser User { get; set; }  // Navigation property to User
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }  // Navigation property
    }
}
