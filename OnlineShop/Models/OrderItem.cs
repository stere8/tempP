namespace OnlineShop.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }  // New: Link to Order
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public Order Order { get; set; }  // New: Navigation property
        public Product Product { get; set; }
    }
}
