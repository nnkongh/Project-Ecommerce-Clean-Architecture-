using Ecommerce.Application.DTOs.Authentication;
using Microsoft.AspNetCore.Http;

namespace Ecommerce.Web.Interface
{
    public interface ICookieTokenService
    {
        string? GetAccessToken();
        void RemoveTokenFromCookie();
        void SetTokenInsideCookie(TokenModel token);
    }
}
