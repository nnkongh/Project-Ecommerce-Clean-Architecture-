namespace Ecommerce.Application.DTOs.Models
{
    public record CommentModel : BaseModel
    {
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string? UserName { get; set; } 
        public string? AvatarUrl { get; set; }
    }
}
