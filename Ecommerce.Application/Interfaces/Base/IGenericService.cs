using Ecommerce.Domain.Specification.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Interfaces.Base
{
    public interface IGenericService<TEntity, TModel> where TEntity : class
    {
        Task<IReadOnlyList<TModel>> GetAllAsync();
        Task<IReadOnlyList<TModel>> GetAsync(Expression<Func<TEntity, bool>> predicate,
                                        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
                                        List<Expression<Func<TEntity, object>>> includes);
        Task<IReadOnlyList<TModel>> GetByAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IReadOnlyList<TModel>> GetAsync(Expression<Func<TEntity, bool>> predicate,
                                        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy);
        Task<TModel> GetEnityWithSpecAsync(ISpecification<TEntity> spec);
        Task<IReadOnlyList<TModel>> GetAsync(ISpecification<TEntity> spec);
        Task<TModel> GetByIdASync(int id);
        Task<TModel> AddAsync(TModel entity);
        Task UpdateAsync(TModel entity);
        Task DeleteAsync(TModel entity);
    }
}
