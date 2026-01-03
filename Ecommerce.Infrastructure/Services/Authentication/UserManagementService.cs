using AutoMapper;
using Ecommerce.Application.DTOs;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Domain.Models;
using Ecommerce.Domain.Shared;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Identity;
using Ecommerce.Infrastructure.Interfaces.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Services.Authentication
{
    public class UserManagementService : IIdentityManagementUserProvider
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public UserManagementService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> AddLoginAsync(AppUser user, UserLoginInfo loginInfo)
        {
            return await _userManager.AddLoginAsync(user, loginInfo);
        }

        public async Task<AppUser> CreateUserAsync(AppUser Appuser, string? password)
        {
            var result = await _userManager.CreateAsync(Appuser, password);
            var role = await _userManager.AddToRoleAsync(Appuser, "User");
            return Appuser;

        }

        public async Task<IdentityResult> CreateUserExternalAsync(AppUser user)
        {
            var result = await _userManager.CreateAsync(user);
            var role = await _userManager.AddToRoleAsync(user, "User");
            return result;
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<Microsoft.AspNetCore.Identity.SignInResult> ExternalLoginSignInAsync(ExternalLoginInfo info)
        {
            return await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: true, bypassTwoFactor: true);
        }

        public async Task<ExternalLoginInfo> GetExternalLoginInformationAsync()
        {
            return await _signInManager.GetExternalLoginInfoAsync() ?? throw new ArgumentException("Error loading external login information");
        }

        public async Task<IList<UserLoginInfo>> GetLoginAsync(AppUser user)
        {
            return await _userManager.GetLoginsAsync(user);
        }

        public async Task SignInAsync(AppUser user, bool isPersistent)
        {
            await _signInManager.SignInAsync(user, isPersistent: true);
        }

        public async Task UpdateUserAsync(AppUser appUser)
        {
            await _userManager.UpdateAsync(appUser);
        }
    }
}
