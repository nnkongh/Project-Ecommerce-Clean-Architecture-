using Ecommerce.Application.DTOs;
using Ecommerce.Application.DTOs.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Interfaces.Authentication
{
    public interface ITokenService
    {
        Task<TokenModel> CreateToken(UserModel user, bool populateExp);
        Task<TokenModel> CreateRefreshToken(TokenModel tokenDto);
    }
}
