using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Models;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Repository.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Repository
{
    public class ShopRepository : GenericRepository<Shop, int>, IShopRepository
    {
        public ShopRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Shop?> GetByUserIdAsync(string userId)
        {
            return await _context.Shops.FirstOrDefaultAsync(s => s.UserId == userId);
        }
    }
}
