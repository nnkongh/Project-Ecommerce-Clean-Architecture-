using Ecommerce.Domain.Specification.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Repository.Base
{
    public class SpecificationEvaluator<T> where T : class
    {
        public static IQueryable<T> GetQuery(IQueryable<T> InputQuery, ISpecification<T> specification)
        {
            var query = InputQuery;
            if(specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
            }
            query = specification.Include.Aggregate(query, (current,include) => current.Include(include));
            query = specification.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));

            if(specification.OrderBy != null)
            {
                query = query.OrderBy(specification.OrderBy);
            }
            if(specification.OrderByDescending != null)
            {
                query = query.OrderByDescending(specification.OrderByDescending);
            }
            if (specification.IsPagingEnabled)
            {
                query = query.Skip(specification.Skip).Take(specification.Take);
            }
            return query;
        }
    }
}
