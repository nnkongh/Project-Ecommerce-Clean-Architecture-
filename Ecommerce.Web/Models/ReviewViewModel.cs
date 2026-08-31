namespace Ecommerce.Web.ViewModels
{
    public class ReviewViewModel
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string? UserName { get; set; }
        public string? AvatarUrl { get; set; }
    }
}
