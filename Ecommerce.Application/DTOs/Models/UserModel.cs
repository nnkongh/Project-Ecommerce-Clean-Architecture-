using Ecommerce.Application.DTOs.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.Models
{
    public record UserModel
    {
        public string Id { get; set; } = default!;
        public required string UserName { get; set; }
        public string? IdentityId { get; set; }
        public AddressModel? Address { get; set; } 
        public string? PhoneNumber { get; set; }
        public bool EmailConfirmed { get; set; }
        public required string Email { get; set; }
        public string? ImageUrl { get; set; }
        public IEnumerable<string>? Role { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime ExpiryRefreshToken { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
