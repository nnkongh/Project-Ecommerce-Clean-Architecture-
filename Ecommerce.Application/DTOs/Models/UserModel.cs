using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs
{
    public class UserModel
    {
        public string Id { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Role { get; set; } = default!;
        public string ImageUrl { get; set; } = default!;
        public string RefreshToken { get; set; } = default!;
        public DateTime ExpiryRefreshToken { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
