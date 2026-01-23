using Ecommerce.Application.DTOs.Models;

namespace Ecommerce.Web.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; } = default!;
        public string? UserName { get; set; }
        public AddressModel? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
