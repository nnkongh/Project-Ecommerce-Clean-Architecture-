namespace Ecommerce.Application.DTOs.Models
{
    public record NotificationModel : BaseModel
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
