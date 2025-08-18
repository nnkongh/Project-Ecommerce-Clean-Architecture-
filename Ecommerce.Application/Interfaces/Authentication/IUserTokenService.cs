using Ecommerce.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Interfaces.Authentication
{
    public interface IUserTokenService
    {
        // token operations
        Task<string> GeneratePasswordResetTokenAsync(UserDto userDto);
        Task<bool> ResetPasswordAsync(string userId, string token, string password);
    }
}
