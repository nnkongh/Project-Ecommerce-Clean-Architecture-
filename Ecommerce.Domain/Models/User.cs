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
        public string Email { get; set; } = default!;
        public string IdentityId { get; set; } = default!;
        public DateTime CreateAt { get; set; }
        public List<Wishlist> Wishlist { get; set; } 
        public Cart? Cart { get; set; }
    }
}
