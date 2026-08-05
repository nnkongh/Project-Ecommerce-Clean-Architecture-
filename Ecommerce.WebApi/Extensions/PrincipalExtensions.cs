using System.Security.Claims;

namespace Ecommerce.WebApi.Extensions
{
    public static class PrincipalExtensions
    {
        public static string? GetUserId(this ClaimsPrincipal principal)
        {
            var claim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (claim != null)
            {
                return claim;
            }
            return null;

        }
    }
}
