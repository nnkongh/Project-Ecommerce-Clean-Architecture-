using Ecommerce.Domain.Models;
using Ecommerce.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Interfaces.Authentication
{
    public interface IIdentityManagementUserProvider
    {
        Task<AppUser> CreateUserAsync(AppUser user,string password);
        Task UpdateUserAsync(AppUser user);
        Task<bool> DeleteUserAsync(string userId);
        Task<AppUser> CreateUserExternalAsync(AppUser user);
        Task<IdentityResult> AddLoginAsync(AppUser user, UserLoginInfo loginInfo);
    }
}
