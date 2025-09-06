using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.Authentication
{
    public class LoginModel
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
