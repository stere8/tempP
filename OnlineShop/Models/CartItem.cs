namespace OnlineShop.Models
{
    public class CartItem
    {
        public int CartItemID { get; set; }
        public int CartId { get; set; }
        public Cart Cart { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}