using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces.Authentication;
using Ecommerce.Infrastructure.Identity;
using Ecommerce.Infrastructure.Interfaces.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Repository.User_Repository
{
    public class UserTokenRepository : IUserTokenService
    {
        private readonly UserManager<AppUser> _userManager;

        public UserTokenRepository(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<string> GeneratePasswordResetTokenAsync(AppUser user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return token;
        }

        public async Task<bool> ResetPasswordAsync(AppUser appUser, string token, string password)
        {
            var result = await _userManager.ResetPasswordAsync(appUser, token, password);
            if (!result.Succeeded)
            {
                var error = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Password reset failed: {error}");
            }
            return false;
        }
    }
}
