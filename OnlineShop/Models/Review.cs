namespace OnlineShop.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public string UserId { get; set; } // Foreign key to ApplicationUser
        public ApplicationUser? User { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public int OrderId { get; set; }  // Link to Order model
        public Order? Order { get; set; }  // Navigation property for Order
        public DateTime CreatedDate { get; set; }
    }

}
