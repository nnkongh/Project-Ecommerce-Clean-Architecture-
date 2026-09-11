using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Models;
using Ecommerce.Domain.Specification;
using Ecommerce.Domain.Specification.Base;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Repository.Base;
using Microsoft.EntityFrameworkCore;
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
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<Order>> GetOrdersByUserIdAsync(string userId)
        {
            var item = new OrderWithItemSpecification(userId);
            return await GetAsync(item);
        }
        public async Task<IReadOnlyList<Order>> GetOrdersByShopIdAsync(int shopId)
        {
            return await _context.Orders
                .Where(o => o.SubOrders.Any(s => s.ShopId == shopId))
                .Include(o => o.SubOrders)
                    .ThenInclude(s => s.Items)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }
        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            var item = new OrderWithItemSpecification(orderId);
            return await GetEnityWithSpecAsync(item);
        }
        public async Task<Order?> GetByIdWithSubOrdersAsync(int orderId)
        {
            return await _context.Orders
                .Where(o => o.Id == orderId)
                .Include(o => o.SubOrders)
                    .ThenInclude(s => s.Items)
                .FirstOrDefaultAsync();
        }
    }
}
