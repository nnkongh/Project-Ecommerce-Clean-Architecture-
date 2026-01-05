using Ecommerce.Application.Common.Command.Authentication.Forgot;
using Ecommerce.Application.Common.Command.Authentication.Login;
using Ecommerce.Application.Common.Command.Authentication.Register;
using Ecommerce.Application.Common.Command.Authentication.Reset;
using Ecommerce.Application.Common.Command.AuthenticationExternal;
using Ecommerce.Application.DTOs.Authentication;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Application.Interfaces;
using Ecommerce.Infrastructure;
using Ecommerce.Infrastructure.Identity;
using Ecommerce.Web.ViewModels.ApiResponse;
using Ecommerce.WebApi.Controllers.BaseController;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.WebApi.Controllers
{
    [Route("auth")]
    public class AuthController : ApiController
    {
        public AuthController(ISender sender) : base(sender)
        {
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel login)
        {
            var command = new LoginCommand(login);
            var result = await Sender.Send(command);
            if (result.IsSuccess)
            {
                return Ok(new ApiResponse<TokenModel>
                {
                    IsSuccess = true,
                    Value = result.Value
                });
            }
            return BadRequest(result.Error);
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel register)
        {
            var command = new RegisterCommand(register);
            var result = await Sender.Send(command);
            if (result.IsFailure)
            {
                return BadRequest(new ApiResponse<string>
                {
                    IsSuccess = false,
                    Error = new ApiError
                    {
                        Code = result.Error.Code,
                        Message = result.Error.Message,
                    }
                });
            }
            return Ok(new ApiResponse<UserModel>
            {
                IsSuccess = true,
                Value = result.Value,
            });
        }

     
        [HttpPost("logout")]
        public IActionResult Logout(ClaimsPrincipal principal, HttpContext context)
        {
            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return BadRequest();
            }
            return Ok();
        }
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
            return result.IsSuccess ? Ok(new ApiResponse<bool> { IsSuccess = true }) : BadRequest(result.Error);
        }
    }
}
