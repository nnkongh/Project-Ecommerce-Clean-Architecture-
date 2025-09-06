using AutoMapper;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces.Authentication;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Domain.Models;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Authen
{
    public class UserManagementRepository : IUserManagementService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserManagementRepository(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, IUnitOfWork uow)
        {
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public async Task<User> CreateUserAsync(UserModel userDto, string password)
        {   
            var userApp = new AppUser
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
            };
           
            var result = await _userManager.CreateAsync(userApp, password);
            var role = await _userManager.AddToRoleAsync(userApp, "User");
            if (!result.Succeeded)
            {
                throw new Exception("User creation failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
            var user = new User
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                ImageUrl = ""
            };
            return user;
            
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user == null)
            {
                throw new ArgumentException("User not found");
            }
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }
        // LOGIC SAI
        public async Task UpdateUserAsync(UserModel userDto)
        {
            var user = await _userManager.FindByIdAsync(userDto.Id);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }
            var mapped = _mapper.Map(userDto, user);
            await _userManager.UpdateAsync(mapped);
        }
    }
}
