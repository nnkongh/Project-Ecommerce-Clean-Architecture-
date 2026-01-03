using Ecommerce.Application.DTOs.Authentication;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Shared;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Interfaces;

public interface ITokenService
{
    Task<TokenModel> CreateToken(UserModel user, bool populateExp);
    Task<TokenModel> RefreshAccessTokenAsync(TokenModel tokenDto);

    ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken);
    ClaimsPrincipal GetPrincipalFromToken(string accessToken);
   // string CreateRefreshToken();
    //Task<Result> RevokeRefreshToken(string userId);
}
