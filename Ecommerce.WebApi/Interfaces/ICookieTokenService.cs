using Ecommerce.Application.DTOs.Authentication;

namespace Ecommerce.WebApi.Interfaces
{
    public interface ICookieTokenService
    {
        void SetTokenInsideCookie(TokenModel token, HttpContext httpContext);
        void RemoveTokenFromCookie(HttpContext httpContext);
    }
}
