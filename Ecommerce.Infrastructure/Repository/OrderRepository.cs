using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Models;
using Ecommerce.Domain.Specification;
using Ecommerce.Domain.Specification.Base;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Repository
{
    public class OrderRepository : GenericRepository<Order, int>, IOrderRepository
    {
        public OrderRepository(EcommerceDbContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<Order>> GetOrdersByUserIdAsync(string userId)
        {
            var item = new OrderWithItemSpecification(userId);
            return await GetAsync(item);
        }
        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            var item = new OrderWithItemSpecification(orderId);
            return await GetEnityWithSpecAsync(item);
        }
    }
}
