using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.Authentication
{
    public class ForgotPasswordDto
    {
        public string Email { get; set; } = default!;
        public string ClientUrl { get; set; } = default!;
    }
}
