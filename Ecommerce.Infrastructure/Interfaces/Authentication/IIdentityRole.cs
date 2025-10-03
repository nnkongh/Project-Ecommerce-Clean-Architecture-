using Ecommerce.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Interfaces.Authentication;

public interface IIdentityRole
{
    Task<IEnumerable<string>> GetRolesAsync(AppUser user);
    Task<bool> IsInRoleAsync(AppUser user, string role);
    Task AddToRoleAsync(AppUser user, string role);
    Task AssignRoleAsync(string role);

}
