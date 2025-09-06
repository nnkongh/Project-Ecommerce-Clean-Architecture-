using Ecommerce.Application.DTOs;
using Ecommerce.Domain.Models;
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
        Task<User?> FindUserNameAsync(string userName);
        Task<User?> FindEmailAsync(string email);
        Task<User?> FindByIdAsync(string id);
        Task<bool> CheckPasswordAsync(string userId, string password);
       
    }
}
