using Ecommerce.Application.DTOs.Models;

namespace Ecommerce.Web.ViewModels.Profile
{
    public class ProfileViewModel
    {
        public string UserName { get; set; } = default!;
        public AddressModel? Address { get; set; }
        public string Email { get; set; } = default!;
        public string? PhoneNumber { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
