using Ecommerce.Domain.Interfaces.Base;
using Ecommerce.Domain.Specification.Base;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Repository.Base
{
    public class GenericRepository<T,TKey> : IRepositoryBase<T,TKey> where T : class
    {
        protected readonly EcommerceDbContext _context;
        public GenericRepository(EcommerceDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public Task<T> AddAsync(T entity)
        {
            _context.Set<T>().Add(entity);
            return Task.FromResult(entity);

        }

        public virtual Task Delete(T entity)
        {
             _context.Set<T>().Remove(entity);
            return Task.FromResult(entity);
        }

        public async Task<IReadOnlyList<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

   
        public async Task<IReadOnlyList<T>> GetByAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public virtual async Task<T?> GetByIdAsync(TKey id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public virtual async  Task<T?> UpdatePartialAsync(TKey id, T entity, params Expression<Func<T,object>>[] propertiesToUpdate)
        {
            var existing = await _context.Set<T>().FindAsync(id);
            if (existing == null) return null;

            if(propertiesToUpdate?.Length > 0)
            {
                
                foreach(var prop in propertiesToUpdate)
                {
                    var compiled = prop.Compile(); // biến một kiểu expression thành một kiểu delegate Func<T,object>
                    var newValue = compiled(entity);
                    _context.Entry(existing).Property(prop).CurrentValue = newValue;
                }
            }
            return entity;
        }
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }
        private IQueryable<T> ApplySpecification(ISpecification<T> specfication)
        {
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(),specfication);
        }
        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> order, List<Expression<Func<T, object>>> includes)
        {
            IQueryable<T> query = _context.Set<T>();
            if (predicate != null)
            {
                query = query.Where(predicate);                
            }
            if(order != null)
            {
                return await order(query).ToListAsync();
            }
            if(includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            return await query.ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy)
        {
            IQueryable<T> query = _context.Set<T>();
            if(predicate != null)
            {
                query = query.Where(predicate);
            }
            if(orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            return await query.ToListAsync();
        }
        
        public async Task<IReadOnlyList<T>> GetAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<T?> GetEnityWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public Task<T> Update(T entity)
        {
            _context.Set<T>().Update(entity);
            return Task.FromResult(entity);
        }
    }
}
