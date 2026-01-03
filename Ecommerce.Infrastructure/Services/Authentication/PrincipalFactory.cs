using Ecommerce.Infrastructure.Interfaces.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Services.Authentication
{
    public class PrincipalFactory : IPrincipalFactory
    {
        private readonly IConfiguration _configuration;
        private readonly SymmetricSecurityKey _key;
        public PrincipalFactory(IConfiguration configuration)
        {
            _configuration = configuration;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        }

        public ClaimsPrincipal CreatePrincipalFromToken(string token)
        {
            var tokenParams = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["Jwt:Issuer"] ?? throw new Exception("Issuer is missing"),
                //ValidAudience = _configuration["Jwt:Audience"] ?? throw new Exception("Audience is missing"),
                IssuerSigningKey = _key,
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenParams, out SecurityToken _);
            return principal;
        }
    }
}
