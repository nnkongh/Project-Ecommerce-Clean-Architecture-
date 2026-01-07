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
    }
}
