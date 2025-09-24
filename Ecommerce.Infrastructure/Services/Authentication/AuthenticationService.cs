using AutoMapper;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.DTOs.Authentication;
using Ecommerce.Application.DTOs.EmailMessage;
using Ecommerce.Application.Interfaces;
using Ecommerce.Application.Interfaces.Authentication;
using Ecommerce.Domain.Models;
using Ecommerce.Domain.Shared;
using Ecommerce.Infrastructure.Exceptions;
using Ecommerce.Infrastructure.Identity;
using Ecommerce.Infrastructure.Interfaces.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IIdentityUserProvider _userAuthenticationService;
        private readonly IUserTokenService _userTokenService;
        private readonly IIdentityManagementUserProvider _userManagementService;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly IIdentityRole _roleManagement;
        public AuthenticationService(IIdentityUserProvider userAuthenticationService,
            IUserTokenService userTokenService,
            IEmailService emailService,
            ITokenService tokenService,
            IMapper mapper,
            IIdentityManagementUserProvider userManagementService,
            IIdentityRole roleManagement)
        {
            _userAuthenticationService = userAuthenticationService;
            _userTokenService = userTokenService;
            _emailService = emailService;
            _mapper = mapper;
            _userManagementService = userManagementService;
            _roleManagement = roleManagement;
        }

        public async Task<Result> ForgotPassword(ForgotPasswordModel forgotPasswordDto)
        {
            try
            {
                // tim kiem nguoi dung theo email
                var user = await _userAuthenticationService.FindEmailAsync(forgotPasswordDto.Email);
                if (user == null || forgotPasswordDto.ClientUrl == null)
                {
                    return Result.Failure<string>(new Error("", "User does not exist"));
                }
                // tạo token, param là 1 dictionary chứa token và email. callback sẽ chứa clientUrl và param. cuối cùng là message sẽ được gửi đến email của người dùng
                var mapped = _mapper.Map<UserModel>(user);
                var token = await _userTokenService.GeneratePasswordResetTokenAsync(user);
                var encodedToken = WebUtility.UrlEncode(token);
                var param = new Dictionary<string, string> { { "token", encodedToken }, { "email", forgotPasswordDto.Email } };
                var callback = QueryHelpers.AddQueryString(forgotPasswordDto.ClientUrl, param!);
                var msgBody = $@"Xin chào {user.UserName} đây là token reset mật khẩu {encodedToken}";
                var msg = new Message([user.Email!], "Reset password", msgBody);
                await _emailService.SendEmail(msg);
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
                var user = await _userAuthenticationService.FindEmailAsync(login.Email);
                if (user == null)
                {
                    return Result.Failure<UserModel>(new Error("", "Email not found"));
                }
                var result = await _userAuthenticationService.CheckPasswordAsync(user, login.Password);
                if(result == false)
                {
                    return Result.Failure<UserModel>(new Error("", "Invalid email or password"));
                }
                var mapped = _mapper.Map<UserModel>(user);
                return Result.Success(mapped);
            }

        public async Task<Result<UserModel>> Register(RegisterModel model)
        {
            if (await UserNameExisting(model.UserName))
            {
                return Result.Failure<UserModel>(new Error("", $"User name {model.UserName} is already exist"));
            }
            var appUser = new AppUser
            {
                UserName = model.UserName,
                Email = model.Email,
                RefreshToken = "",
            };
           
            var result = await _userManagementService.CreateUserAsync(appUser, model.Password);
            if(result  == null)
            {
                return Result.Failure<UserModel>(new Error("", "Some error occured while processing"));
            };
            var role = await _roleManagement.GetRolesAsync(result);
            var roles = role.ToList();
            var userModel = new UserModel
            {
                Email = model.Email,
                UserName = model.UserName,
                ImageUrl = "",
                CreatedAt = DateTime.Now,
                RefreshToken = "",
                IdentityId = result.Id,
                Id = result.Id,
                Role = roles
            };
            return Result.Success(userModel);
        }

        private async Task<bool> UserNameExisting(string userName)
        {
            var user = await _userAuthenticationService.FindUserNameAsync(userName);
            if(user == null)
            {
                return false;
            }
            return true;

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
                var decodedToken = WebUtility.UrlDecode(model.Token);
                var result = await _userTokenService.ResetPasswordAsync(user, decodedToken, model.NewPassword);
                if(result == false)
                {
                    return Result.Failure(new Error("", "Token invalid"));
                }
                await _userManagementService.UpdateUserAsync(user);
                return Result.Success();
            }
            catch (Exception)
            {
                return Result.Failure(new Error("", "An unexpected error occurred"));
            }
        }
    }
}
