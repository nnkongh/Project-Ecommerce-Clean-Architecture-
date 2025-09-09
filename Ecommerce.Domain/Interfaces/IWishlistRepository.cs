using Ecommerce.Domain.Interfaces.Base;
using Ecommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Interfaces
{
    public interface IWishlistRepository : IRepositoryBase<Wishlist,int>
    {
        Task<bool> GetExistingProduct(string userId, int productI);
        Task<Wishlist?> GetWishlistWithItemByIdAsync(int id);
        Task<Wishlist?> GetByUserIdAsync(string userId);
    }
}
