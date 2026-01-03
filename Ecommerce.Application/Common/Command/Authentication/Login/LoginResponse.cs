using Ecommerce.Application.DTOs.Authentication;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Models;
using Ecommerce.Domain.Shared;
using Microsoft.Win32.SafeHandles;

namespace Ecommerce.Application.Common.Command.Authentication.Login
{
    public class LoginResponse
    {
        public TokenModel Token { get; set; }
        public UserModel User { get; set; }
    }
}