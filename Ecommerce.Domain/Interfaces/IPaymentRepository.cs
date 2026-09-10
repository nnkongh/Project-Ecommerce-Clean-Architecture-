using Ecommerce.Domain.Interfaces.Base;
using Ecommerce.Domain.Models;

namespace Ecommerce.Domain.Interfaces
{
    public interface IPaymentRepository : IRepositoryBase<Payment, int>
    {
        Task<Payment?> GetBySubOrderIdAsync(int subOrderId);
    }
}