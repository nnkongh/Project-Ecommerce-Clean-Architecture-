using Ecommerce.Application.DTOs.Authentication;
using Ecommerce.Web.Interface;
using Microsoft.AspNetCore.Http;

namespace Ecommerce.Infrastructure;

public class CookieTokenService : ICookieTokenService
{
    private readonly IHttpContextAccessor _contextAccessor;

    public CookieTokenService(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public string? GetAccessToken()
    {
        var context = _contextAccessor.HttpContext;
        if(context == null)
        {
            return null;
        }
        context.Request.Cookies.TryGetValue("access_token", out var token);
        return token;
    }

    public void RemoveTokenFromCookie()
    {
        var context = _contextAccessor.HttpContext;
        if (context == null)
        {
            throw new InvalidOperationException("Httpcontext is not available");
        }
        context.Response.Cookies.Append("refresh_token", "", new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddDays(-1)
        });
        context.Response.Cookies.Append("access_token", "", new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddDays(-1)
        });
    }

    public void SetTokenInsideCookie(TokenModel token)
    {
        var context = _contextAccessor.HttpContext;
        if (context == null)
        {
            throw new InvalidOperationException("Httpcontext is not available");
        }
        context.Response.Cookies.Append("access_token", token.AccessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddMinutes(15)
        });
        context.Response.Cookies.Append("refresh_token", token.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddDays(1)
        });
    }
}
