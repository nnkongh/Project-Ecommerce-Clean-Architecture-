using Ecommerce.Application.DTOs;
using Ecommerce.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Interfaces.Authentication
{
    public interface IUserTokenService
    {
        // token operations
        Task<string> GeneratePasswordResetTokenAsync(AppUser appUser);
        Task<bool> ResetPasswordAsync(AppUser appUser, string token, string password);
    }
}
