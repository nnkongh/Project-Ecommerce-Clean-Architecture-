using Ecommerce.Domain.Interfaces.Base;
using Ecommerce.Domain.Models;

namespace Ecommerce.Domain.Interfaces
{
    public interface ICartRepository  : IRepositoryBase<Cart,int>
    {
        Task<Cart?> GetCartWithItemByUserIdAsync(string userId);
        Task<Cart?> GetCartWithItemByIdAsync(int cartId);
    }
}
