using Ecommerce.Application.DTOs.Authentication;

namespace Ecommerce.Web.Interface
{
    public interface ICookieTokenService
    {
        void RemoveTokenFromCookie(HttpContext httpContext);
        void SetTokenInsideCookie(TokenModel token, HttpContext httpContext);
    }
}
