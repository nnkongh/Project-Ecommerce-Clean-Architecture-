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
            return result.IsSuccess ? Ok(new ApiResponse<TokenModel> { Value = result.Value, IsSuccess = true}) 
                                    : BadRequest(new ApiResponse<TokenModel> { IsSuccess = false, Error = result.Error});
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel register)
        {
            var command = new RegisterCommand(register);
            var result = await Sender.Send(command);
            return result.IsSuccess ? Ok(new ApiResponse<UserModel> { Value = result.Value, IsSuccess = true}) 
                                    : BadRequest(new ApiResponse<TokenModel> { IsSuccess = false, Error = result.Error });
        }
     
        [HttpPost("logout")]
        public IActionResult Logout(ClaimsPrincipal principal, HttpContext context)
        {
            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            return userId == null ? BadRequest() : Ok();
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel forgotPasswordDto)
        {
            var command = new ForgotPasswordCommand(forgotPasswordDto);
            var result = await Sender.Send(command);
            return result.IsSuccess ? Ok(new ApiResponse<UserModel> { IsSuccess = true }) 
                                    : BadRequest(new ApiResponse<TokenModel> { IsSuccess = false, Error = result.Error });
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel resetPasswordDto)
        {
            var command = new ResetPasswordCommand(resetPasswordDto);
            var result = await Sender.Send(command);
            return result.IsSuccess ? Ok(new ApiResponse<UserModel> { IsSuccess = true }) 
                                    : BadRequest(new ApiResponse<TokenModel> { IsSuccess = false, Error = result.Error });
        }
    }
}
