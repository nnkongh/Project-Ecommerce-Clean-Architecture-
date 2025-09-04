namespace Ecommerce.Domain.Models
{
    public class ProductWishList
    {
        public int Id { get; set; } = default!;
        public int ProductId { get; set; }
        public Product Product { get; set; } = default!;
        public int WishListId { get; set; }
        public Wishlist WishList { get; set; } = default!;
    }
}