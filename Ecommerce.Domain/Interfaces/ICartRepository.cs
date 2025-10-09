using Ecommerce.Domain.Interfaces.Base;
using Ecommerce.Domain.Models;

namespace Ecommerce.Domain.Interfaces
{
    public interface ICartRepository  : IRepositoryBase<Cart,int>
    {
        Task<Cart?> GetCartWithItemByUserIdAsync(string userId);
        Task<Cart?> GetCartWithItemByIdAsync(int cartId);


        //System
        Task<List<Cart>> GetExpiredCartsAsync(DateTime currentTime, CancellationToken cancellationToken);
        void UpdateRange(IEnumerable<Cart> carts, CancellationToken cancellationToken);
        void DeleteRange(IEnumerable<Cart> carts, CancellationToken cancellationToken);

    }
}
