using Ecommerce.Domain.Enum;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.UnitOfWork;
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
    public class CartRepository : GenericRepository<Cart,int>, ICartRepository
    {
        public CartRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<Cart?> GetCartWithItemByUserIdAsync(string userId)
        {
            var spec = new CartWithItemsSpecification(userId);
            var result = await GetAsync(spec);
            return result.Count > 0 ? result[0] : null;
        }
        public async Task<Cart?> GetCartWithItemByIdAsync(int cartId)
        {
            var spec = new CartWithItemsSpecification(cartId);
            var result = await GetAsync(spec);
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<Cart>> GetExpiredCartsAsync(DateTime currentTime)
        {
            return await _context.Carts.Where(x => x.ExpiredAt <= currentTime && x.Status == CartStatus.Active).ToListAsync();
        }

        public void UpdateRange(IEnumerable<Cart> carts)
        {
            _context.UpdateRange(carts);
        }

        public void DeleteRange(IEnumerable<Cart> carts)
        {
            _context.RemoveRange(carts);
        }
    }
}
