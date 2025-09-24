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

namespace Ecommerce.Infrastructure.Services.Authentication
{
    public class UserAuthenticationService : IIdentityUserProvider
    {
        private readonly UserManager<AppUser> _userManager;
        public UserAuthenticationService(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
        }
        public async Task<bool> CheckPasswordAsync(AppUser user, string password)
            => await _userManager.CheckPasswordAsync(user, password);

        public async Task<AppUser?> FindByIdAsync(string id)
            => await _userManager.FindByIdAsync(id);

        public async Task<AppUser?> FindEmailAsync(string email)
            => await _userManager.FindByEmailAsync(email);

        public async Task<AppUser?> FindUserNameAsync(string userName)
            => await _userManager.FindByNameAsync(userName);
    }
}
