using AutoMapper;
using Ecommerce.Application.Interfaces.Base;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Models;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Identity;
using Ecommerce.Infrastructure.Repository.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {

        public UserRepository(EcommerceDbContext context) : base(context)
        {
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var user = await _context.Set<User>().FirstOrDefaultAsync(x => x.Email == email);
            if (user == null) return null;
            return user;
        }

        public async Task<User?> GetUserByIdAsync(string id)
        {
            var user = await _context.Set<User>().FirstOrDefaultAsync(x => x.Id == id);
            if (user == null) return null;
            return user;
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            var user = await _context.Set<User>().FirstOrDefaultAsync(x => string.Equals(x.UserName,username,StringComparison.OrdinalIgnoreCase));
            if (user == null) return null;
            return user;
        }
    }
}
