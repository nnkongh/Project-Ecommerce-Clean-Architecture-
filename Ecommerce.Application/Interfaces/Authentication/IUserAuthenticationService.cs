using Ecommerce.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Interfaces.Authentication
{
    public interface IUserAuthenticationService
    {
        // Login/Authentication
        Task<UserDto> FindUserNameAsync(string userName);
        Task<UserDto> FindEmailAsync(string email);
        Task<UserDto> FindByIdAsync(string id);
        Task<bool> CheckPasswordAsync(string userId, string password);
       
    }
}
