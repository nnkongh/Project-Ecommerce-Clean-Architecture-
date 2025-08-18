using AutoMapper;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.DTOs.Authentication;
using Ecommerce.Application.DTOs.EmailMessage;
using Ecommerce.Application.Interfaces;
using Ecommerce.Application.Interfaces.Authentication;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Exceptions;
using Ecommerce.Infrastructure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Services.Authen
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;
        private readonly IUserAuthenticationService _userAuthenticationService;
        private readonly IUserManagementService _userManagermentService;
        private readonly IUserRoleService _userRoleService;
        private readonly IUserTokenService _userTokenService;
        private readonly SymmetricSecurityKey _key;

        public AuthenticationService(IUserAuthenticationService userAuthenticationService,
            IEmailService emailService,
            IConfiguration config,
            IUserManagementService userManagermentService,
            IUserRoleService userRoleService,
            IUserTokenService userTokenService)
        {
            _userManagermentService = userManagermentService;
            _userRoleService = userRoleService;
            _userTokenService = userTokenService;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
            _config = config;
            _emailService = emailService;
            _userAuthenticationService = userAuthenticationService;
        }

        public async Task<TokenDto> CreateRefreshToken(TokenDto tokenDto)
        {
            var principal = GetPrincipalFromExpiredToken(tokenDto.AccessToken);
            var user = await _userAuthenticationService.FindUserNameAsync(principal.Identity?.Name!);
            if (user == null || user.RefreshToken != tokenDto.RefreshToken || user.ExpiryRefreshToken < DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException("Invalid refresh token");
            }
            return await CreateToken(user, populateExp: false);
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken)
        {
            var tokenParams = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = false,
                ValidAudience = _config["Jwt:Audience"],
                ValidIssuer = _config["Jwt:Issuer"],
                IssuerSigningKey = _key
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(accessToken, tokenParams, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken is null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }
            return principal;
        }

        public async Task<TokenDto> CreateToken(UserDto userDto, bool populateExp)
        {

            // Tạo claim cho token
            var claims = await GetClaims(userDto);
            var refreshToken = GenerateRefreshToken();
            userDto.RefreshToken = refreshToken;
            if (populateExp)
            {
                userDto.ExpiryRefreshToken = DateTime.UtcNow.AddDays(7);
            }
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature);
            // tạo token descriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = creds,
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"],
                Expires = DateTime.UtcNow.AddDays(1)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(token);
            await _userManagermentService.UpdateUserAsync(userDto);
            return new TokenDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }

        private async Task<List<Claim>> GetClaims(UserDto userDto)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, userDto.Email!),
                new Claim(JwtRegisteredClaimNames.Sub, userDto.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, userDto.Id!),
                new Claim(ClaimTypes.Name, userDto.UserName!),
            };
            var role = await _userRoleService.GetRolesAsync(userDto.Id);
            claims.AddRange(role.Select(role => new Claim(ClaimTypes.Role, role)));
            return claims;  
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }
            return Convert.ToBase64String(randomNumber);
        }

        public async Task ForgotPassword(ForgotPasswordDto forgotPasswordDto)
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
                var token = await _userTokenService.GeneratePasswordResetTokenAsync(user);
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

        public async Task<TokenDto> Login(LoginDto login, HttpContext context)
        {
            if (string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.Password))
            {
                throw new ArgumentNullException("Email or Password cannot be null or empty");
            }
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
                var token = await CreateToken(user, true);
                SetTokenInsideCookie(token, context);
                return token;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while processing the login request.", ex);
            }

        }

        public async Task Register(RegisterDto register)
        {
            if (string.IsNullOrEmpty(register.UserName) || string.IsNullOrEmpty(register.Email) || string.IsNullOrEmpty(register.Password) || string.IsNullOrEmpty(register.ConfirmPassword))
            {
                throw new ArgumentNullException("UserName, Email, Password or ConfirmPassword cannot be null or empty");
            };
            if (!string.Equals(register.Password, register.ConfirmPassword, StringComparison.Ordinal))
            {
                throw new("Password and ConfirmPassword do not match");
            }
            try
            {
                var user = new UserDto
                {
                    UserName = register.UserName,
                    Email = register.Email,
                    CreatedAt = DateTime.UtcNow,
                    RefreshToken = "",
                    
                };
                var result = await _userManagermentService.CreateUserAsync(user,register.Password);
                
            } catch (Exception ex)
            {
                // Log the exception (ex) here if needed
                throw new Exception("An error occurred while processing the registration request.", ex);
            };
        }

        public void RemoveTokenFromCookie(HttpContext context)
        {
            context.Response.Cookies.Append("refresh_token", "", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMinutes(-1)
            });
        }

        public async Task ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            if (string.IsNullOrEmpty(resetPasswordDto.Email) || string.IsNullOrEmpty(resetPasswordDto.NewPassword) || string.IsNullOrEmpty(resetPasswordDto.ConfirmPassword))
            {
                throw new ArgumentNullException($"{nameof(resetPasswordDto)} can not null");
            }
            try
            {
                var user = await _userAuthenticationService.FindEmailAsync(resetPasswordDto.Email);
                var token = await _userTokenService.GeneratePasswordResetTokenAsync(user);
                var result = await _userTokenService.ResetPasswordAsync(user.Id, token, resetPasswordDto.NewPassword);
                await _userManagermentService.UpdateUserAsync(user);
            }
            catch (Exception ex) {
                throw new Exception($"Error: {ex.Message}" );
            }
        }

        public void SetTokenInsideCookie(TokenDto tokenDto, HttpContext context)
        {
            context.Response.Cookies.Append("access_token", tokenDto.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(1)
            });
        }

        public async Task Logout(ClaimsPrincipal principal, HttpContext context)
        {
            var userClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
            var user = await _userAuthenticationService.FindByIdAsync(userClaim.Value);

            user.RefreshToken = null;
            user.ExpiryRefreshToken = DateTime.MinValue;

            await _userManagermentService.UpdateUserAsync(user);
            RemoveTokenFromCookie(context);

        }
    }
}
