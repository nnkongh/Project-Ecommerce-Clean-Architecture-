using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.Models
{
    public class ProfileModel
    {
        public string UserName { get; set; } = default!;
        public AddressModel? Address { get; set; }
        public string Email { get; set; } = default!;
        public string? ImageUrl { get; set; }
    }
}
