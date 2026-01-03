using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Identity
{
    public class AppUser : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        
        public string GoogleId { get; set; }
        public bool EmailConfirmed { get; set; }
        public string FullName { get; set; }
    }
}
