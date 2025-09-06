using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Models
{
    public class User
    {
        public string Id { get; set; } = default!;
        public string? ImageUrl { get; set; }
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Address { get; set; } = default!;
        public DateTime CreateAt { get; set; }
        public string PhoneNumber { get; set; } = default!;
        public Wishlist? Wishlist { get; set; } 
        public Cart? Cart { get; set; }
    }
}
