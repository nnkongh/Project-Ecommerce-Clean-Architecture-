using Ecommerce.Domain.Models;
using Ecommerce.Domain.Specification.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Specification
{
    public class OrderWithItemSpecification : BaseSpecification<Order>
    {
        public OrderWithItemSpecification(string userId) : base(o => o.CustomerId == userId)
        {
            AddIncludes(o => o.Items);
        }
        public OrderWithItemSpecification(int orderId) : base(o => o.Id == orderId)
        {
            AddIncludes(o => o.Items);
        }
        public OrderWithItemSpecification() : base(null)
        {
            AddIncludes(o => o.Items);
        }
    }
}
