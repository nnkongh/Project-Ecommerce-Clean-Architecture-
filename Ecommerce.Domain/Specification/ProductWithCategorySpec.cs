using Ecommerce.Domain.Models;
using Ecommerce.Domain.Specification.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Specification
{
    public sealed class ProductWithCategorySpec : BaseSpecification<Product>
    {
        public ProductWithCategorySpec() : base() { }

        public ProductWithCategorySpec(int parentCategoryId)
            : base(p => p.Category.ParentId == parentCategoryId) 
        { 
            AddIncludes(p => p.Name);
            AddIncludes(p => p.Category);
        }

        public ProductWithCategorySpec(string name) : base(p => p.Name.ToLower().Contains(name.ToLower())) 
        {
            AddIncludes(p => p.Category);
        }
        public ProductWithCategorySpec(int categoryId, bool isCategoryId) : base(p => p.CategoryId == categoryId || p.Category.ParentId == categoryId) 
        {
            AddOrderBy(p => p.Name);
            AddIncludes(p => p.Category);
        }

    }

}