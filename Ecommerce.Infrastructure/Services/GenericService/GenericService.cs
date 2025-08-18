using AutoMapper;
using Ecommerce.Application.Interfaces.Base;
using Ecommerce.Domain.Interfaces.Base;
using Ecommerce.Domain.Specification.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Services.GenericService
{
    public class GenericService<TModel,TEntity> : IGenericService<TEntity, TModel> where TEntity : class
    {
        private readonly IRepositoryBase<TEntity> _repositoryBase;
        private readonly IMapper _mapper;

        public GenericService(IRepositoryBase<TEntity> repositoryBase, IMapper mapper) {
            _repositoryBase = repositoryBase;
            _mapper = mapper;
        }
        public async Task<TModel> AddAsync(TModel model)
        {
            var entity = _mapper.Map<TEntity>(model);
            var item = await _repositoryBase.AddAsync(entity);
            var mapped = _mapper.Map<TModel>(entity);
            return mapped;
        }
        public async Task DeleteAsync(TModel model)
        {
            var entity = _mapper.Map<TEntity>(model);
            await _repositoryBase.DeleteAsync(entity);
        }
        public async Task<IReadOnlyList<TModel>> GetAllAsync()
        {
            var entity = await _repositoryBase.GetAllAsync();
            var mapped = _mapper.Map<IReadOnlyList<TModel>>(entity);
            return mapped;
        }

        public async Task<IReadOnlyList<TModel>> GetAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, List<Expression<Func<TEntity, object>>> includes)
        {
            var specific = await _repositoryBase.GetAsync(predicate,orderBy, includes);
            var mapped = _mapper.Map<IReadOnlyList<TModel>>(specific);
            return mapped;
        }

        public async Task<IReadOnlyList<TModel>> GetAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
        {
            var specific = await _repositoryBase.GetAsync(predicate, orderBy);
            var mapped = _mapper.Map<IReadOnlyList<TModel>>(specific);
            return mapped;
        }

        public async Task<IReadOnlyList<TModel>> GetAsync(ISpecification<TEntity> spec)
        {
            var specific = await _repositoryBase.GetAsync(spec);
            var mapped = _mapper.Map<IReadOnlyList<TModel>>(specific);
            return mapped;
        }

        public async Task<IReadOnlyList<TModel>> GetByAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var specific = await _repositoryBase.GetByAsync(predicate);
            var mapped = _mapper.Map<IReadOnlyList<TModel>>(specific);
            return mapped;
        }

        public async Task<TModel> GetByIdASync(int id)
        {
            var entity = await _repositoryBase.GetByIdASync(id);
            var mapped = _mapper.Map<TModel>(entity);
            return mapped;
        }

        public async Task<TModel> GetEnityWithSpecAsync(ISpecification<TEntity> spec)
        {
            var entity = await _repositoryBase.GetEnityWithSpecAsync(spec);
            var mapped = _mapper.Map<TModel>(entity); 
            return mapped;
        }

        public async Task UpdateAsync(TModel model)
        {
            var entity = _mapper.Map<TEntity>(model);
            await _repositoryBase.UpdateAsync(entity);
        }
    }
}
