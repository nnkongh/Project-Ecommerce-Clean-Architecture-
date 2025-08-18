using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Interfaces.Authentication
{
    public interface IUserRoleService
    {
        Task<IEnumerable<string>> GetRolesAsync(string userId);
        Task<bool> IsInRoleAsync(string userId, string role);
        
    }
}
