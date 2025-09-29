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
    public class WishlistRepository : GenericRepository<Wishlist, int>, IWishlistRepository
    {
        public WishlistRepository(EcommerceDbContext context) : base(context)
        {
            
        }

        public async Task<Wishlist?> GetWishlistWithItemByIdAsync(int wishListId)
        {
            var spec = new WishListWithItemsSpecification(wishListId);
            return await GetEnityWithSpecAsync(spec);
        }

        public async Task<IReadOnlyList<Wishlist>> GetWishlistsWithItemByUserIdAsync(string userId)
        {
            var spec = new WishListWithItemsSpecification(userId);
            return await GetAsync(spec);
        }

        public async Task<Wishlist> GetWishlistWithItemByUserId(string userId)
        {
            var spec = new WishListWithItemsSpecification(userId);
            return await GetEnityWithSpecAsync(spec);
        }
    }
}
