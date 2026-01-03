using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.Authentication
{
    public class ExternalLoginInfoDto
    {
        public string Provider { get; set; } = default!;
        public string ProviderKey { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string? Name { get; set; }
        public string? DisplayName { get; set; }

    }
}
