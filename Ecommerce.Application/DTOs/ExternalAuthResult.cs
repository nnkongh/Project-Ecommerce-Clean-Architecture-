namespace Ecommerce.Application.DTOs
{
    public class ExternalAuthResult
    {
        public bool IsSuccess { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string GoogleId { get; set; }
        public string? PictureUrl { get; set; }
    }
}