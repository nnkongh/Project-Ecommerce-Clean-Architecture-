using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Models;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repository
{
    public class UserAddressRepository : GenericRepository<UserAddress, int>, IUserAddressRepository
    {
        public UserAddressRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<UserAddress>> GetAddressesByUserIdAsync(string userId)
        {
            return await _context.Set<UserAddress>()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.IsDefault)
                .ToListAsync();
        }

        public async Task<UserAddress?> GetDefaultAddressAsync(string userId)
        {
            return await _context.Set<UserAddress>()
                .FirstOrDefaultAsync(x => x.UserId == userId && x.IsDefault);
        }
    }
}
