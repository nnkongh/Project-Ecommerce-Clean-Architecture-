using Ecommerce.Application.DTOs.Product;
using Ecommerce.Application.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Interfaces.Core
{
    public interface IProductService
    {
        Task<IEnumerable<ProductModel>> GetProductByName(string productName);
        Task<IEnumerable<ProductModel>> GetProductByCategory(int categoryId);
        Task<ProductModel> GetProductById(int productId);
        Task<ProductModel> Create(ProductModel model);
        Task Update(ProductModel model);
        Task Delete(ProductModel model);
        Task<IEnumerable<ProductModel>> GetProductList();
    }
}
