using Ecommerce.Application.Common.Command.Auth.Forgot;
using Ecommerce.Application.Common.Command.Auth.Login;
using Ecommerce.Application.Common.Command.Auth.Reset;
using Ecommerce.Application.Common.Command.Users.CreateUser;
using Ecommerce.Application.DTOs.Authentication;
using Ecommerce.Application.Interfaces.Authentication;
using Ecommerce.WebApi.Controllers.BaseController;
using Ecommerce.WebApi.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.Web.Controllers
{
    [Route("auth")]
    public class AuthController : ApiController
    {
        private readonly ICookieTokenService _cookieTokenService;
        public AuthController(ISender sender, ICookieTokenService cookieTokenService) : base(sender)
        {
            _cookieTokenService = cookieTokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel login)
        {
            var command = new LoginCommand(login);
            var result = await Sender.Send(command);
            if (result.IsSuccess)
            {
                _cookieTokenService.SetTokenInsideCookie(result.Value, HttpContext);
                return Ok(result.Value);
            }
            return BadRequest(result.Error);
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel register)
        {
            var  command = new RegisterCommand(register);
            var result = await Sender.Send(command);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
        //[HttpPost("logout")]
        //public async Task<IActionResult> Logout(ClaimsPrincipal principal, HttpContext context)
        //{
        //    var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        //    _cookieTokenService.RemoveTokenFromCookie()
        //    if(userId == null)
        //    {
        //        return BadRequest();
        //    }
        //    return Ok();
        //}
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel forgotPasswordDto)
        {
            var command = new ForgotPasswordCommand(forgotPasswordDto);
            var result = await Sender.Send(command);
            return result.IsSuccess ? Ok(result) : BadRequest(result.Error);
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel resetPasswordDto)
        {
            var command = new ResetPasswordCommand(resetPasswordDto);
            var result = await Sender.Send(command);
            return result.IsSuccess ? Ok(result.IsSuccess) : BadRequest(result.Error);
        }
    }
}
