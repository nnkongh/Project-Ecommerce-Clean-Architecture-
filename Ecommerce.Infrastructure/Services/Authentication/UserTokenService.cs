using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces;
using Ecommerce.Infrastructure.Identity;
using Ecommerce.Infrastructure.Interfaces.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Services.Authentication
{
    public class UserTokenService : IUserTokenService
    {
        private readonly UserManager<AppUser> _userManager;

        public UserTokenService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<string> GeneratePasswordResetTokenAsync(AppUser user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
            
        }

        public async Task<bool> ResetPasswordAsync(AppUser appUser, string token, string password)
        {
            var result = await _userManager.ResetPasswordAsync(appUser, token, password);
            return result.Succeeded;
        }
    }
}
