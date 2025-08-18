using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Models;
using Ecommerce.Domain.Specification;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Repository
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        public CartRepository(EcommerceDbContext context) : base(context)
        {
        }
        public async Task<Cart> GetCartByUserIdAsync(string userName)
        {
            var spec = new CartWithItemsSpecification(userName);
            return (await GetAsync(spec)).FirstOrDefault();
        }
    }
}
