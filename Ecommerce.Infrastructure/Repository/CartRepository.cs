using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Models;
using Ecommerce.Domain.Specification;
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
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        public CartRepository(EcommerceDbContext context) : base(context)
        {   
        }
        public async Task<Cart?> GetCartByUserIdAsync(string userId)
        {
            var spec = new CartWithItemsSpecification(userId);
            var result = await GetAsync(spec);
            return result.Count > 0 ? result[0] : null;
        }
        public async Task<Cart?> GetCartByIdAsync(int cartId)
        {
            var spec = new CartWithItemsSpecification(cartId);
            var result = await GetAsync(spec);
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<IReadOnlyCollection<CartItem?>> GetCartItemAsync(int cartId)
        {
            var list = await _context.Set<CartItem>().Where(c => c.CartId == cartId).AsNoTracking().ToListAsync();
            return list;
        }
    }
}
