using Ecommerce.Application.DTOs;
using Ecommerce.Application.DTOs.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Interfaces.Authentication
{
    public interface IAuthenticationService
    {
        void SetTokenInsideCookie(TokenDto tokenDto, HttpContext context);
        void RemoveTokenFromCookie(HttpContext context);
        Task<TokenDto> Login(LoginDto login, HttpContext context);
        Task Register(RegisterDto register);
        Task ForgotPassword(ForgotPasswordDto forgotPasswordDto);
        Task ResetPassword(ResetPasswordDto resetPasswordDto);  
        Task<TokenDto> CreateToken(UserDto user, bool populateExp);
        Task<TokenDto> CreateRefreshToken(TokenDto tokenDto);
        Task Logout(ClaimsPrincipal principal, HttpContext context);
    }
}
