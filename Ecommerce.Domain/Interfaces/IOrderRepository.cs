using Ecommerce.Domain.Interfaces.Base;
using Ecommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Interfaces
{
    public interface IOrderRepository : IRepositoryBase<Order, int>
    {
        Task<IEnumerable<Order>> GetOrdersByUserNameAsync(string userName);
    }
}
    