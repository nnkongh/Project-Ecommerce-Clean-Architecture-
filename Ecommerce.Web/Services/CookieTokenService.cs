using Ecommerce.Application.DTOs.Authentication;
using Ecommerce.Web.Interface;

namespace Ecommerce.Web.Services
{
    public class CookieTokenService : ICookieTokenService
    {
        private readonly ILogger<CookieTokenService> _logger;

        public CookieTokenService(ILogger<CookieTokenService> logger)
        {
            _logger = logger;
        }

        public void RemoveTokenFromCookie(HttpContext httpContext)
        {
            httpContext.Response.Cookies.Append("refresh_token", "", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(-1)
            });
            httpContext.Response.Cookies.Append("access_token", "", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(-1)
            });
        }

        public void SetTokenInsideCookie(TokenModel token, HttpContext httpContext)
        {
            _logger.LogWarning("Bat dau set token trong cookie");
            httpContext.Response.Cookies.Append("access_token", token.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMinutes(15)
            });
            httpContext.Response.Cookies.Append("refresh_token", token.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(1)
            });
        }
    }
}
