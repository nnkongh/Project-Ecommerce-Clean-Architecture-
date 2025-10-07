using Ecommerce.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.Authentication
{
    public class ExternalLoginModel
    {
        public ProviderType ProviderType { get; set; }
        public string Token { get; set; } = default!;
    }
}
