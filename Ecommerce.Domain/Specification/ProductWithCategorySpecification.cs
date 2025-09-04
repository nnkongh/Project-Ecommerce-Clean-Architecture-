using Ecommerce.Domain.Models;
using Ecommerce.Domain.Specification.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Specification
{
    public class ProductWithCategorySpecification : BaseSpecification<Product>
    {
        public ProductWithCategorySpecification(string productName) : base(p => p.Name.ToLower().Contains(productName.ToLower()))
        {
            AddIncludes(p => p.Category);
        }
        public ProductWithCategorySpecification(int productId) : base(p => p.Id == productId)
        {
            AddIncludes(p => p.Category);
        }
        public ProductWithCategorySpecification() : base(null)
        {
            AddIncludes(p => p.Category);
        }
        
    }
}
