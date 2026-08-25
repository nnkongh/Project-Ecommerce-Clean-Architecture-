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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.WebApi.Controllers
{
    [Route("auth")]
    public class AuthController : ApiController
    {
        private readonly ITokenService _tokenService;

        public AuthController(ISender sender, ITokenService tokenService) : base(sender)
        {
            _tokenService = tokenService;
        }

        [HttpPost("login")] 
        public async Task<IActionResult> Login(LoginModel login)
        {
            var command = new LoginCommand(login);
            var result = await Sender.Send(command);
            return result.IsSuccess
                ? Ok(new ApiResponse<TokenModel> { IsSuccess = true, Value = result.Value })
                : BadRequest(new ApiResponse<TokenModel> { IsSuccess = false, Error = result.Error });
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel register)
        {
            var command = new RegisterCommand(register);
            var result = await Sender.Send(command);
            return result.IsSuccess
                ? Ok(new ApiResponse<UserModel> { IsSuccess = true, Value = result.Value })
                : BadRequest(new ApiResponse<UserModel> { IsSuccess = false, Error = result.Error });
        }
        [Authorize]
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
            return result.IsSuccess ? Ok(result) : BadRequest(result.Error);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(TokenModel tokenModel)
        {
            if (tokenModel is null || string.IsNullOrEmpty(tokenModel.AccessToken) || string.IsNullOrEmpty(tokenModel.RefreshToken))
            {
                return BadRequest(new ApiResponse<TokenModel> { IsSuccess = false, Message = "Invalid token" });
            }

            try
            {
                var result = await _tokenService.RefreshAccessTokenAsync(tokenModel);
                return Ok(new ApiResponse<TokenModel> { IsSuccess = true, Value = result });
            }
            catch (Exception)
            {
                return Unauthorized(new ApiResponse<TokenModel> { IsSuccess = false, Message = "Invalid token" });
            }
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new ApiResponse<string> { IsSuccess = false, Message = "Invalid token" });
            }

            var result = await _tokenService.RevokeRefreshToken(userId);
            return result.IsSuccess
                ? Ok(new ApiResponse<string> { IsSuccess = true, Message = "Logged out successfully" })
                : BadRequest(new ApiResponse<string> { IsSuccess = false, Message = "Logout failed", Error = result.Error });
        }
    }
}
