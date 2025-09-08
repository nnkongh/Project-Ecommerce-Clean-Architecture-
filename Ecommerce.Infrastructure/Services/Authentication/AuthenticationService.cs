using AutoMapper;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.DTOs.Authentication;
using Ecommerce.Application.DTOs.EmailMessage;
using Ecommerce.Application.Interfaces;
using Ecommerce.Application.Interfaces.Authentication;
using Ecommerce.Domain.Models;
using Ecommerce.Domain.Shared;
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

        public async Task<Result> ForgotPassword(ForgotPasswordModel forgotPasswordDto)
        {
            try
            {
                // tim kiem nguoi dung theo email
                var user = await _userAuthenticationService.FindEmailAsync(forgotPasswordDto.Email);
                if (user == null || forgotPasswordDto.ClientUrl == null)
                {
                    return Result.Failure(new Error("", "User does not exist"));
                }
                // tạo token, param là 1 dictionary chứa token và email. callback sẽ chứa clientUrl và param. cuối cùng là message sẽ được gửi đến email của người dùng
                var mapped = _mapper.Map<UserModel>(user);
                var token = await _userTokenService.GeneratePasswordResetTokenAsync(mapped);
                var param = new Dictionary<string, string> { { "token", token }, { "email", forgotPasswordDto.Email } };
                var callback = QueryHelpers.AddQueryString(forgotPasswordDto.ClientUrl, param!);
                var msg = new Message([user.Email!], "Reset password", callback);
                _emailService.SendEmail(msg);
                return Result.Success();

            }
            catch (Exception ex)
            {
                // Log the exception (ex) here if needed
                throw new Exception("An error occurred while processing the forgot password request.", ex);
            }
        }


        public async Task<Result<UserModel>> Login(LoginModel login)
        {
            try
            {
                var user = await _userAuthenticationService.FindEmailAsync(login.Email);
                if (user == null)
                {
                    return Result.Failure<UserModel>(new Error("", "Email not found"));
                }
                var result = await _userAuthenticationService.CheckPasswordAsync(user.Id, login.Password);
                if(result == false)
                {
                    return Result.Failure<UserModel>(new Error("", "Invalid email or password"));
                }
                var mapped = _mapper.Map<UserModel>(user);
                return Result.Success(mapped);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while processing the login request.", ex);
            }

        }

        public async Task<Result> Register(RegisterModel model)
        {
            var userDto = new UserModel
            {
                UserName = model.UserName,
                Email = model.Email,
                CreatedAt = DateTime.UtcNow,
                RefreshToken = "",
            };
            var result = await _userManagementService.CreateUserAsync(userDto, model.Password);
            if(result  == null)
            {
                //
                return Result.Failure(new Error("", "Some "));
            }
            return Result.Success();
        }

        public async Task<Result> ResetPassword(ResetPasswordModel model)
        {
            try
            {
                var user = await _userAuthenticationService.FindEmailAsync(model.Email);
                if(user == null)
                {
                    return Result.Failure(new Error("", "Email does not exist"));
                }
                var mapped = _mapper.Map<UserModel>(user);
                var token = await _userTokenService.GeneratePasswordResetTokenAsync(mapped);
                var result = await _userTokenService.ResetPasswordAsync(user.Id, token, model.NewPassword);
                if(result == false)
                {
                    return Result.Failure(new Error("", "Reset password failed"));
                }
                await _userManagementService.UpdateUserAsync(mapped);
                return Result.Success();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }
    }
}
