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
    public class OrderWithUsernameSpecification : BaseSpecification<Order>
    {
        public OrderWithUsernameSpecification(string username) : base(o => o.CustomerName == username)
        {
            AddIncludes(o => o.CustomerName);
        }

    }
}
