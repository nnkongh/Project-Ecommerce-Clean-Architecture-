using Ecommerce.Application.DTOs.Models;

namespace Ecommerce.Web.Models
{
    public class UserAddressViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; } = default!;
        public string Street { get; set; } = default!;
        public string Ward { get; set; } = default!;
        public string District { get; set; } = default!;
        public string City { get; set; } = default!;
        public string? Province { get; set; }
        public bool IsDefault { get; set; }
    }
}
