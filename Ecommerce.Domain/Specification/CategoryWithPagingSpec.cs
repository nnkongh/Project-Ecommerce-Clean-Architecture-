using Ecommerce.Domain.Models;
using Ecommerce.Domain.Specification.Base;

namespace Ecommerce.Domain.Specification
{
    public sealed class CategoryWithPagingSpec : BaseSpecification<Category>
    {
        public CategoryWithPagingSpec(int? parentId, int pageIndex, int pageSize)
            : base(c => c.ParentId == parentId)
        {
            AddOrderBy(c => c.Name);
            ApplyPagin(pageSize * (pageIndex - 1), pageSize);
        }
    }

    public sealed class CategoryCountSpec : BaseSpecification<Category>
    {
        public CategoryCountSpec(int? parentId) : base(c => c.ParentId == parentId)
        {
        }
    }
}
