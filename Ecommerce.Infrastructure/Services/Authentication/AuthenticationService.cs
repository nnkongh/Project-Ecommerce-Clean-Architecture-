using AutoMapper;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.DTOs.Authentication;
using Ecommerce.Application.DTOs.EmailMessage;
using Ecommerce.Application.Interfaces;
using Ecommerce.Application.Interfaces.Authentication;
using Ecommerce.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserAuthenticationService _userAuthenticationService;
        private readonly IUserTokenService _userTokenService;
        private readonly IUserManagementService _userManagementService;
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        public AuthenticationService(IUserAuthenticationService userAuthenticationService, IUserTokenService userTokenService, IEmailService emailService, ITokenService tokenService, IMapper mapper, IUserManagementService userManagementService)
        {
            _userAuthenticationService = userAuthenticationService;
            _userTokenService = userTokenService;
            _emailService = emailService;
            _tokenService = tokenService;
            _mapper = mapper;
            _userManagementService = userManagementService;
        }

        public async Task ForgotPassword(ForgotPasswordModel forgotPasswordDto)
        {
            if (string.IsNullOrEmpty(forgotPasswordDto.Email) || string.IsNullOrEmpty(forgotPasswordDto.ClientUrl))
            {
                throw new ArgumentNullException("Email or ClientUrl cannot be null or empty");
            }
            try
            {
                // tim kiem nguoi dung theo email
                var user = await _userAuthenticationService.FindEmailAsync(forgotPasswordDto.Email);
                if (user == null || forgotPasswordDto.ClientUrl == null)
                {
                    throw new ArgumentNullException("User is null");
                }
                // tạo token, param là 1 dictionary chứa token và email. callback sẽ chứa clientUrl và param. cuối cùng là message sẽ được gửi đến email của người dùng
                var mapped = _mapper.Map<UserModel>(user);
                var token = await _userTokenService.GeneratePasswordResetTokenAsync(mapped);
                var param = new Dictionary<string, string> { { "token", token }, { "email", forgotPasswordDto.Email } };
                var callback = QueryHelpers.AddQueryString(forgotPasswordDto.ClientUrl, param!);
                var msg = new Message([user.Email!], "Reset password", callback);
                _emailService.SendEmail(msg);

            }
            catch (Exception ex)
            {
                // Log the exception (ex) here if needed
                throw new Exception("An error occurred while processing the forgot password request.", ex);
            }
        }


        public async Task<TokenModel> Login(LoginModel login)
        {
            try
            {
                var user = await _userAuthenticationService.FindEmailAsync(login.Email);
                if (user == null)
                {
                    throw new NotFoundException(nameof(user));
                }
                var result = await _userAuthenticationService.CheckPasswordAsync(user.Id, login.Password);
                if (!result)
                {
                    throw new UnauthorizedAccessException("Invalid email or password");
                }
                var mapped = _mapper.Map<UserModel>(user);
                var token = await _tokenService.CreateToken(mapped, true);
                return token;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while processing the login request.", ex);
            }

        }

        public async Task<UserModel> Register(RegisterModel model)
        {
            var userDto = new UserModel
            {
                UserName = model.UserName,
                Email = model.Email,
                CreatedAt = DateTime.UtcNow,
                RefreshToken = "",
            };
            var result = await _userManagementService.CreateUserAsync(userDto, model.Password);
            return userDto;
        }

        public async Task ResetPassword(ResetPasswordModel model)
        {
            try
            {
                var user = await _userAuthenticationService.FindEmailAsync(model.Email);
                var mapped = _mapper.Map<UserModel>(user);
                var token = await _userTokenService.GeneratePasswordResetTokenAsync(mapped);
                var result = await _userTokenService.ResetPasswordAsync(user.Id, token, model.NewPassword);
                await _userManagementService.UpdateUserAsync(mapped);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }
    }
}
