namespace OnlineShop.Models
{
    public class WishlistItem
    {
        public int WishlistItemId { get; set; }
        public int WishlistId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
