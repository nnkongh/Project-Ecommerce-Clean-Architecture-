using Ecommerce.Domain.Models;
using Ecommerce.Domain.Specification.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Specification.ProductSpec
{
    public class ProductSpecification : BaseSpecification<Product>
    {
        protected ProductSpecification() { }

        protected ProductSpecification(Expression<Func<Product, bool>> criteria) : base(criteria)
        {
            AddIncludes(p => p.Category);
        }
    }
}
