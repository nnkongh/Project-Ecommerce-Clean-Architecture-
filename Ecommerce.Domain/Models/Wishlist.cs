namespace Ecommerce.Domain.Models
{
    public class Wishlist
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public List<ProductWishList> ProductsWishList { get; set; } = new List<ProductWishList>();
    }
}