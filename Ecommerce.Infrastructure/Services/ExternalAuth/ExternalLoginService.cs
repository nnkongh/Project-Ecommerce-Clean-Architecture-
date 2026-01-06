using AutoMapper;
using Ecommerce.Application.DTOs.Authentication;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Enum;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Domain.Models;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Exceptions;
using Ecommerce.Infrastructure.Identity;
using Ecommerce.Infrastructure.Interfaces.Authentication;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Services.ExternalAuth
{
    public class ExternalLoginService : IExternalLoginService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserRepository _userRespository;
        private readonly IIdentityRole _roleManager;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<ExternalLoginService> _logger;
        public ExternalLoginService(IMapper mapper, UserManager<AppUser> userManager, IUserRepository userRepository, IIdentityRole roleManager, IUserRepository userRespository, IUnitOfWork uow, ILogger<ExternalLoginService> logger)
        {
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _userRespository = userRespository;
            _uow = uow;
            _logger = logger;
        }

        public async Task AddExternalLoginToExistingUserAsync(string userId, ExternalIdentity externalIdentity)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException("Failed to add external login");
            }
            var loginInfo = new UserLoginInfo(externalIdentity.Provider, externalIdentity.ProviderKey, externalIdentity.Provider);
            var result = await _userManager.AddLoginAsync(user, loginInfo);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Failed to add external login: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }

        public async Task<UserModel> CreateUserFromExternalAsync(ExternalUserInfo externalUserInfo, ExternalIdentity externalIdentity, CancellationToken cancellationToken)
        {
            var appUser = new AppUser
            {
                UserName = externalUserInfo.Email,
                Email = externalUserInfo.Email,
                EmailConfirmed = true,
            };
            var result = await _userManager.CreateAsync(appUser);
            var roles = await _roleManager.GetRolesAsync(appUser);
            var role = roles.ToList();
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Failed to create user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            var loginInfo = new UserLoginInfo(externalIdentity.Provider, externalIdentity.ProviderKey, externalIdentity.Provider);

            var loginResult = await _userManager.AddLoginAsync(appUser, loginInfo);
            if (!loginResult.Succeeded)
            {
                await _userManager.DeleteAsync(appUser);
                throw new InvalidOperationException("Failed to create user");
            }
            var user = new UserModel
            {
                Id = appUser.Id,
                Email = externalUserInfo.Email,
                UserName = externalUserInfo.Name,
                EmailConfirmed = true,
                IdentityId = appUser.Id,
                Role = role
            };
            var mapped = _mapper.Map<User>(user);
            await _userRespository.AddAsync(mapped);
            await _roleManager.AddToRoleAsync(appUser, "User");
            await _uow.SaveChangesAsync(cancellationToken);
            return user;
        }

        public async Task<UserModel?> FindByExternalLoginAsync(string provider, string providerKey)
        {
            var user = await _userManager.FindByLoginAsync(provider, providerKey);
            return user == null ? null : _mapper.Map<UserModel>(user);
        }
    }
}
