using Ecommerce.Application.DTOs.Authentication;
using Ecommerce.Application.Interfaces.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.Web.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel login)
        {
            var token = await _authenticationService.Login(login, HttpContext);
            return Ok(token);
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel register)
        {
            await _authenticationService.Register(register);
            return Ok();
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout(ClaimsPrincipal principal, HttpContext context)
        {
            await _authenticationService.Logout(principal, context);
            return Ok();
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel forgotPasswordDto)
        {
            await _authenticationService.ForgotPassword(forgotPasswordDto);
            return Ok();
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel resetPasswordDto)
        {
            await _authenticationService.ResetPassword(resetPasswordDto);
            return Ok();
        }
    }
}
