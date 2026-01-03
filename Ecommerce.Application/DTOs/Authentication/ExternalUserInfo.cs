using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.Authentication
{
    public class ExternalUserInfo
    {
        public string Provider { get; init; }
        public string ProviderKey { get; init; }
        public string Email { get; init; }
        public string Name { get; init; }

    }
}
