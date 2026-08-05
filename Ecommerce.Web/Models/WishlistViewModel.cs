namespace Ecommerce.Web.ViewModels
{
    public class WishlistViewModel
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<ItemWishlistViewModel> Items { get; set; } = new();
    }

    public class ItemWishlistViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public int WishListId { get; set; }
    }
}