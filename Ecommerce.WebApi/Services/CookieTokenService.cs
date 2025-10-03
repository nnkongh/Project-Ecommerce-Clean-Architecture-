using Ecommerce.Application.DTOs.Authentication;
using Ecommerce.WebApi.Interfaces;

namespace Ecommerce.WebApi.Services
{
    public class CookieTokenService : ICookieTokenService
    {
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
