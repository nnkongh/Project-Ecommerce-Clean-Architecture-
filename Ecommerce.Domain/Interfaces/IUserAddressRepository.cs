using Ecommerce.Domain.Interfaces.Base;
using Ecommerce.Domain.Models;

namespace Ecommerce.Domain.Interfaces
{
    public interface IUserAddressRepository : IRepositoryBase<UserAddress, int>
    {
        Task<IReadOnlyList<UserAddress>> GetAddressesByUserIdAsync(string userId);
        Task<UserAddress?> GetDefaultAddressAsync(string userId);
    }
}
