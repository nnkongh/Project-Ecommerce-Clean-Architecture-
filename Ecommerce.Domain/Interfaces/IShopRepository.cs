using Ecommerce.Domain.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Interfaces
{
    public interface IShopRepository : IRepositoryBase<Shop,int>
    {
        Task<Shop?> GetByUserIdAsync(string userId);
    }
}
