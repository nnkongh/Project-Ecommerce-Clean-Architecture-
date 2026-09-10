using Ecommerce.Domain.Interfaces.Base;
using Ecommerce.Domain.Models;

namespace Ecommerce.Domain.Interfaces
{
    public interface ISubOrderRepository : IRepositoryBase<SubOrder, int>
    {
        Task<IReadOnlyList<SubOrder>> GetByOrderIdAsync(int orderId);
        Task<IReadOnlyList<SubOrder>> GetByShopIdAsync(int shopId);
        Task<SubOrder?> GetByIdWithItemsAsync(int subOrderId);
        Task<SubOrder?> GetByIdWithPaymentAsync(int subOrderId);
    }
}