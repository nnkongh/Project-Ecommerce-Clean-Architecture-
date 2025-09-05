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
        public async Task<Wishlist> GetByIdAsync(int wishListId)
        {
            var spec = new WishListWithItemsSpecification(wishListId);
            return (await GetAsync(spec)).FirstOrDefault();
        }

        public Task<Wishlist?> GetByUserIdAndProductIdAsync(string userId, int productI)
        {
            throw new NotImplementedException();
        }
    }
}
