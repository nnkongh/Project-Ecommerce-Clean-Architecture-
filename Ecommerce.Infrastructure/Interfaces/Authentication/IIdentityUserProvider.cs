using Ecommerce.Application.DTOs;
using Ecommerce.Domain.Enum;
using Ecommerce.Domain.Models;
using Ecommerce.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Interfaces.Authentication
{
    public interface IIdentityUserProvider
    {
        // Login/Authentication
        Task<AppUser?> FindUserNameAsync(string userName);
        Task<AppUser?> FindEmailAsync(string email);
        Task<AppUser?> FindByIdAsync(string id);
        Task<bool> CheckPasswordAsync(AppUser user, string password);
        Task<AppUser?> GetProviderAsync(ProviderType providerType, string providerUserId);
    }
}
