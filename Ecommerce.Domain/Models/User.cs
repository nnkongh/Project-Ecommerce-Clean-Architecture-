using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Models
{
    public class User
    {
        public string Id { get; set; } = default!;
        public string? UserName { get; set; }
        public string? ImageUrl { get; set; }
        public string Email { get; set; } = default!;
        public string IdentityId { get; set; } = default!;
        public DateTime CreateAt { get; set; }
        public Address? Address { get; set; }
        public List<Wishlist?> Wishlist { get; set; } = [];
        public List<Cart?> Cart { get; set; } = [];
        public List<Order> Orders { get; set; } = [];
    }
}
