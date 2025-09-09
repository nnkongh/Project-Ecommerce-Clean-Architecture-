using Ecommerce.Domain.Interfaces.Base;
using Ecommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Interfaces
{
    public interface IProductRepository : IRepositoryBase<Product, int>
    {
        Task<IEnumerable<Product?>> GetProductByNameAsync(string name);
        Task<Product?> GetProductByIdWithCategoryAsync(int productId);
        Task<IEnumerable<Product?>> GetProductByCategoryIdAsync(int categoryId);
        Task<IEnumerable<Product>> GetProductByCategoryAsync();
    }
}
