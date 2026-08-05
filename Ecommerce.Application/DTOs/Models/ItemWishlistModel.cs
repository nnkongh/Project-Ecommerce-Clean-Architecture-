namespace Ecommerce.Application.DTOs.Models
{
    public record ItemWishlistModel : BaseModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int WishListId { get; set; }
        public string? ImageUrl { get; set; }
    }
}
