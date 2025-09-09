using AutoMapper;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces.Authentication;
using Ecommerce.Domain.Models;
using Ecommerce.Infrastructure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Authen
{
    public class UserAuthenticationRepository : IIdentityUserProvider
    {
        private readonly UserManager<AppUser> _userManager;
        public UserAuthenticationRepository(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
        }
        public async Task<bool> CheckPasswordAsync(string userId, string password)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<AppUser?> FindByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return user;
        }

        public async Task<AppUser?> FindEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user;
        }

        public async Task<AppUser?> FindUserNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            return user;
        }
    }
}
