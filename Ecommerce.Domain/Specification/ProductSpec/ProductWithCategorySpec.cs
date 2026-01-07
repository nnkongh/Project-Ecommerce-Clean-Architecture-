using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Specification.ProductSpec
{
    public sealed class ProductWithCategorySpec : ProductSpecification
    {
        public ProductWithCategorySpec() : base() { }

        public ProductWithCategorySpec(int id)
            : base(p => p.Id == id) { }

        public ProductWithCategorySpec(string name)
            : base(p => p.Name.ToLower().Contains(name.ToLower())) { }

        
    }

}