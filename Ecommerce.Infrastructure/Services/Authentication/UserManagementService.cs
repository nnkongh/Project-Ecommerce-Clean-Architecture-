using AutoMapper;
using Ecommerce.Application.DTOs;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Domain.Models;
using Ecommerce.Infrastructure.Data;
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
    public class UserManagementService : IIdentityManagementUserProvider
    {
        private readonly UserManager<AppUser> _userManager;

        public UserManagementService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> AddLoginAsync(AppUser user, UserLoginInfo loginInfo)
        {
            return await _userManager.AddLoginAsync(user, loginInfo);
        }

        public async Task<AppUser> CreateUserAsync(AppUser Appuser, string password)
        {
            var result = await _userManager.CreateAsync(Appuser,password);
            var role = await _userManager.AddToRoleAsync(Appuser, "User");
            return Appuser;
            
        }

        public async Task<AppUser> CreateUserExternalAsync(AppUser user)
        {
            var result = await _userManager.CreateAsync(user);
            var role = await _userManager.AddToRoleAsync(user, "User");
            return user;
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user == null)
            {
                throw new ArgumentException("User not found");
            }
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }
        // LOGIC SAI
        public async Task UpdateUserAsync(AppUser appUser)
        {
            var user = await _userManager.FindByIdAsync(appUser.Id);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }
            await _userManager.UpdateAsync(user);
        }
    }
}
