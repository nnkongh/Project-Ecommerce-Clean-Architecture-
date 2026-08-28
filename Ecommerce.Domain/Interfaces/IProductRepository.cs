using Ecommerce.Domain.Interfaces.Base;
using Ecommerce.Domain.Models;
using Ecommerce.Domain.Specification.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Interfaces
{
    public interface IProductRepository : IRepositoryBase<Product, int>
    {
        Task<IReadOnlyList<Product>> GetProductsByIdsAsync(IEnumerable<int> ids);
        Task<IReadOnlyList<Product>> GetProductsByCategoryIdAsync(int categoryId);
        Task<IReadOnlyList<Product>> GetProductsByShopIdAsync(int shopId);
    }
}
