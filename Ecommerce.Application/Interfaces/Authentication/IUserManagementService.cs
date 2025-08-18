using Ecommerce.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Interfaces.Authentication
{
    public interface IUserManagementService
    {
        Task<UserDto> CreateUserAsync(UserDto userDto,string password);
        Task UpdateUserAsync(UserDto userDto);
        Task<bool> DeleteUserAsync(string userId);
    }
}
