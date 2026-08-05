
namespace Ecommerce.Domain.Models
{
    public class ItemWishList
    {
        public int Id { get; private set; } = default!;
        public string? ProductName { get; private set; }
        public int ProductId { get; private set; }
        public string? ImageUrl { get; private set; }
        public int WishListId { get; private set; }
        public Wishlist WishList { get; private set; } = default!;
        public Product Product { get; private set; } = default!;

        public static ItemWishList Create(int productId, string productName, string? imageUrl = null)
        {
            return new ItemWishList
            {
                ProductId = productId,
                ProductName = productName,
                ImageUrl = imageUrl
            };
        }
    }
}