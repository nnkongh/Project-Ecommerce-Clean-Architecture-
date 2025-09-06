using Ecommerce.Application.DTOs;
using Ecommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Interfaces.Authentication
{
    public interface IUserManagementService
    {
        Task<User> CreateUserAsync(UserModel userDto,string password);
        Task UpdateUserAsync(UserModel userDto);
        Task<bool> DeleteUserAsync(string userId);
    }
}
