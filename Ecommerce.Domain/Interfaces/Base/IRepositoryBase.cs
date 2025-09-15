using Ecommerce.Domain.Specification.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Interfaces.Base
{
    public interface IRepositoryBase<T,TKey> where T : class
    {
        Task<T?> GetByIdAsync(TKey id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate,
                                        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
                                        List<Expression<Func<T,object>>> includes);
        Task<IReadOnlyList<T>> GetByAsync(Expression<Func<T, bool>> predicate);
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T,bool>> predicate,
                                        Func<IQueryable<T>,IOrderedQueryable<T>> orderBy);
        Task<T?> GetEnityWithSpecAsync(ISpecification<T> spec);
        Task<IReadOnlyList<T>> GetAsync(ISpecification<T> spec);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(TKey id, T entity, params Expression<Func<T, object>>[] propertiesToUpdate);
        Task<bool> Delete(TKey id);
    }
}
