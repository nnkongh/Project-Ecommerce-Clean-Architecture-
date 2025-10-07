using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.Authentication
{
    public sealed record ExternalUserInfo(string ProviderId, string Email, string? Name);
}
