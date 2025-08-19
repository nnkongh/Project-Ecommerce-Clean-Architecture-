using Ecommerce.Domain.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Interfaces.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        IProductRepository ProductRepository { get; }
        ICartRepository CartRepository { get; }
        IOrderRepository OrderRepository { get; }
    }
}
