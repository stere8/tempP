namespace OnlineShop.Models
{
    public class Wishlist
    {
        public int WishlistId { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<WishlistItem> WishlistItems { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
