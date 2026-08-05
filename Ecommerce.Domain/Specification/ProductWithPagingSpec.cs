using Ecommerce.Domain.Interfaces;
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
    public sealed class ProductWithPagingSpec : BaseSpecification<Product>
    {
        public ProductWithPagingSpec(int pageIndex, int pageSize) : base(p => p.IsActive)
        {
            AddOrderBy(p => p.Name);
            ApplyPagin(pageSize * (pageIndex - 1), pageSize);   
        }
    }
    public sealed class ProductCountSpec : BaseSpecification<Product>
    {
        public ProductCountSpec() : base(p => p.IsActive)
        {
        }
    }
    public sealed class ProductCountByCategorySpec : BaseSpecification<Product>
    { 
        public ProductCountByCategorySpec(int categoryId) : base(p => p.IsActive && p.CategoryId == categoryId)
        {
        }
    }
}
