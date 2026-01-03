using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Interfaces.Authentication
{
    public interface IPrincipalFactory
    {
        ClaimsPrincipal CreatePrincipalFromToken(string token);
    }
}
