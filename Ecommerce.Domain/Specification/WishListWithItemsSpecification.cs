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
    public class WishListWithItemsSpecification : BaseSpecification<Wishlist>
    {
        public WishListWithItemsSpecification(string userId) : base(p => p.UserId == userId)
        {
            AddIncludes(p => p.Items);
        }
        public WishListWithItemsSpecification(int wishListId) : base(p => p.Id == wishListId)
        {
            AddIncludes(p => p.Items);
        }
    }
}
