using Ecommerce.Domain.Interfaces.Base;
using Ecommerce.Domain.Models;

namespace Ecommerce.Domain.Interfaces
{
    public interface IReviewRepository : IRepositoryBase<Review, int>
    {
        Task<Review?> GetByIdWithUserAsync(int id);
        Task<IEnumerable<Review>> GetAllReviewsByProductIdAsync(int productId);
    }
}
