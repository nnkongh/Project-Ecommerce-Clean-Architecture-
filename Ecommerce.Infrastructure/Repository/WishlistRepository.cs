using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Models;
using Ecommerce.Domain.Specification;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Repository.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Repository
{
    public class WishlistRepository : GenericRepository<Wishlist>, IWishlistRepository
    {
        public WishlistRepository(EcommerceDbContext context) : base(context)
        {
            
        }
        //public async Task<Wishlist> GetByUserIdAndProductIdAsync(string userId, int productId)
        //{
        //    return await _context.WishtList.FirstOrDefaultAsync(w => w.UserId == userId && w.Pr);
        //}
        public async Task<Wishlist?> GetWishlistWithItemByIdAsync(int wishListId)
        {
            var spec = new WishListWithItemsSpecification(wishListId);
            var result = await GetAsync(spec);
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<Wishlist?> GetByUserIdAsync(string userId)
        {
            var spec = new WishListWithItemsSpecification(userId);
            var result = await GetAsync(spec);
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<bool> GetExistingProduct(string userId, int productId)
        {
            return await _context.Wishlist.AnyAsync(w => w.UserId == userId && w.Items.Any(w => w.ProductId == productId));
        }
    }
}
