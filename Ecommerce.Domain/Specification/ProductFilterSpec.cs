using System.Linq.Expressions;
using Ecommerce.Domain.Models;
using Ecommerce.Domain.Specification.Base;

namespace Ecommerce.Domain.Specification
{
    public sealed class ProductFilterSpec : BaseSpecification<Product>
    {
        public ProductFilterSpec(string? sortBy, decimal? minPrice, decimal? maxPrice, int? categoryId, string? searchTerm)
            : base(BuildCriteria(minPrice, maxPrice, categoryId, searchTerm))
        {
            sortBy = sortBy?.ToLower();
            switch (sortBy)
            {
                case "price_asc":
                    AddOrderBy(p => p.Price);
                    break;
                case "price_desc":
                    AddOrderByDescending(p => p.Price);
                    break;
                case "name_asc":
                    AddOrderBy(p => p.Name);
                    break;
                case "name_desc":
                    AddOrderByDescending(p => p.Name);
                    break;
                case "newest":
                    AddOrderByDescending(p => p.Id);
                    break;
                default:
                    AddOrderBy(p => p.Name);
                    break;
            }
        }

        private static Expression<Func<Product, bool>> BuildCriteria(
            decimal? minPrice, decimal? maxPrice, int? categoryId, string? searchTerm)
        {
            Expression<Func<Product, bool>> criteria = p => p.IsActive;

            if (!string.IsNullOrEmpty(searchTerm))
                criteria = criteria.AndAlso(p => p.Name.Contains(searchTerm));

            if (minPrice.HasValue)
                criteria = criteria.AndAlso(p => p.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                criteria = criteria.AndAlso(p => p.Price <= maxPrice.Value);

            if (categoryId.HasValue)
                criteria = criteria.AndAlso(p => p.CategoryId == categoryId.Value);

            return criteria;
        }
    }
}
