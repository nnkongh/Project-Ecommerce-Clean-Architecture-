using Ecommerce.Application.DTOs.Authentication;
using Microsoft.AspNetCore.Http;

namespace Ecommerce.Infrastructure
{
    public interface ICookieTokenService
    {
        void RemoveTokenFromCookie(HttpContext httpContext);
        void SetTokenInsideCookie(TokenModel token, HttpContext httpContext);
    }
}
