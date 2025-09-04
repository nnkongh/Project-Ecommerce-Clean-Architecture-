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
    public class CartWithItemsSpecification : BaseSpecification<Cart>
    {
        public CartWithItemsSpecification(string userId) : base(p => p.UserId == userId)
        {
            AddIncludes(p => p.Items);
        }
        public CartWithItemsSpecification(int cartId) : base(p => p.Id == cartId)
        {
            AddIncludes(p => p.Items);
        }
    }
}
