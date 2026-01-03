using Ecommerce.Domain.Models;
using Ecommerce.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Interfaces.Authentication
{
    public interface IIdentityUserProvider
    {
        // Login/Authentication
        Task<AppUser?> FindUserNameAsync(string userName);
        Task<AppUser?> FindEmailAsync(string email);
        Task<AppUser?> FindByIdAsync(string id);
        Task<bool> CheckPasswordAsync(AppUser user, string password);
        Task<AppUser?> GetProviderAsync(string providerType, string providerUserId);
    }
}
