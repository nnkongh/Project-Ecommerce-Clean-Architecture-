using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Models;
using Ecommerce.Domain.Specification;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Repository.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Repository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(EcommerceDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Product?>> GetProductByCategoryIdAsync(int categoryId)
        {
            return await _context.Products
                .Where(x => x.CategoryId == categoryId).ToListAsync();
        }

        public async Task<Product?> GetProductByIdWithCategoryAsync(int productId)
        {
            var spec = new ProductWithCategorySpecification(productId);
            return await GetEnityWithSpecAsync(spec);
        }

        public async Task<IEnumerable<Product?>> GetProductByNameAsync(string name)
        {
            var spec = new ProductWithCategorySpecification(name);
            var result = await GetAsync(spec);
            return result;
        }

        public async Task<IEnumerable<Product>> GetProductByCategoryAsync()
        {
            var spec = new ProductWithCategorySpecification();
            return await GetAsync(spec);
        }
    }
}
