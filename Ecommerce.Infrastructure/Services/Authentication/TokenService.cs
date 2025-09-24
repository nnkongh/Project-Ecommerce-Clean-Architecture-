using AutoMapper;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.DTOs.Authentication;
using Ecommerce.Application.DTOs.EmailMessage;
using Ecommerce.Application.Interfaces;
using Ecommerce.Application.Interfaces.Authentication;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Shared;
using Ecommerce.Infrastructure.Exceptions;
using Ecommerce.Infrastructure.Identity;
using Ecommerce.Infrastructure.Interfaces.Authentication;
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
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly IIdentityUserProvider _userAuthenticationService;
        private readonly IIdentityManagementUserProvider _userManagermentService;
        private readonly IIdentityRole _userRoleService;
        private readonly SymmetricSecurityKey _key;
        private readonly IMapper _mapper;

        public TokenService(IIdentityUserProvider userAuthenticationService,
            IConfiguration config,
            IIdentityManagementUserProvider userManagermentService,
            IIdentityRole userRoleService,
            IUserTokenService userTokenService,
            IMapper mapper,
            IUserRepository userRepository)
        {
            _userManagermentService = userManagermentService;
            _userRoleService = userRoleService;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
            _config = config;
            _userAuthenticationService = userAuthenticationService;
            _mapper = mapper;
        }

        public async Task<TokenModel> CreateRefreshToken(TokenModel tokenDto)
        {
            var principal = GetPrincipalFromExpiredToken(tokenDto.AccessToken);
            var user = await _userAuthenticationService.FindUserNameAsync(principal.Identity?.Name!);
            var mapped = _mapper.Map<UserModel>(user);
            if (user == null || mapped.RefreshToken != tokenDto.RefreshToken || mapped.ExpiryRefreshToken < DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException("Invalid refresh token");
            }
            return await CreateToken(mapped, populateExp: false);
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

        public async Task<TokenModel> CreateToken(UserModel userDto, bool populateExp)
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
            var mapped = _mapper.Map<AppUser>(userDto);
            await _userManagermentService.UpdateUserAsync(mapped);
            return new TokenModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }

        private async Task<List<Claim>> GetClaims(UserModel userDto)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, userDto.Email!),
                new Claim(JwtRegisteredClaimNames.Sub, userDto.Id!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Name, userDto.UserName!),
            };
            var appUser = _mapper.Map<AppUser>(userDto);
            var role = await _userRoleService.GetRolesAsync(appUser);
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

        //public async Task<Result> RevokeRefreshToken(string userId)
        //{
        //    var user = await _userRepository.GetUserByIdAsync(userId);
        //    if(user == null)
        //    {
        //        return Result.Failure(new Error("", "User does not exist"));
        //    }
        //    var mapped = _mapper.Map<AppUser>(user);
        //    await _userRepository.UpdateAsync(userId,mapped, x => x.);
        //}
    }
}
