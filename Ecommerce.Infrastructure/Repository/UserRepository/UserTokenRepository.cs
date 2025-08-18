using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces.Authentication;
using Ecommerce.Infrastructure.Identity;
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
        public async Task<string> GeneratePasswordResetTokenAsync(UserDto userDto)
        {
            var user = await _userManager.FindByIdAsync(userDto.Id);
            if (user == null) throw new ArgumentNullException("User is null");
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return token;
        }

        public async Task<bool> ResetPasswordAsync(string userId, string token, string password)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;
            var result = await _userManager.ResetPasswordAsync(user, token, password);
            if (!result.Succeeded)
            {
                var error = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Password reset failed: {error}");
            }
            return false;
        }
    }
}
