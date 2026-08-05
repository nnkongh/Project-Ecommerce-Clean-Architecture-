namespace Ecommerce.Application.DTOs.Models
{
    public record WishlistModel : BaseModel
    {
        public string? UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public IReadOnlyList<ItemWishlistModel> Items { get; set; } = new List<ItemWishlistModel>();
    }
}
