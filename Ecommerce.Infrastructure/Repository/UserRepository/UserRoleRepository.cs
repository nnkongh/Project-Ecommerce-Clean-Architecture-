using Ecommerce.Application.Interfaces.Authentication;
using Ecommerce.Infrastructure.Identity;
using Ecommerce.Infrastructure.Interfaces.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Repository.User_Repository
{
    public class UserRoleRepository : IIdentityRole
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public UserRoleRepository(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager) { _roleManager = roleManager; _userManager = userManager; }

        public async Task AddToRoleAsync(AppUser user, string role)
            => await _userManager.AddToRoleAsync(user, role);

        public async Task AssignRoleAsync(string role)
        {
            var identityRole = new IdentityRole(role);
            await _roleManager.CreateAsync(identityRole);
        }

        public async Task<IEnumerable<string>> GetRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return Enumerable.Empty<string>();
            var roles = await _userManager.GetRolesAsync(user);
            return roles;
            
        }

        public async Task<bool> IsInRoleAsync(AppUser user, string role)
        {
            return await _userManager.IsInRoleAsync(user, role);
        }
    }
}
