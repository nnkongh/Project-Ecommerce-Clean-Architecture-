using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Shared;
using Ecommerce.Domain.Specification;
using MediatR;

namespace Ecommerce.Application.Common.Queries.Category.GetRootCategoriesPaged
{
    public sealed class GetRootCategoriesPagedHandler : IRequestHandler<GetRootCategoriesPagedQuery, Result<PagedResult<CategoryModel>>>
    {
        private readonly ICategoryRepository _repo;
        private readonly IMapper _mapper;

        public GetRootCategoriesPagedHandler(ICategoryRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<CategoryModel>>> Handle(GetRootCategoriesPagedQuery request, CancellationToken cancellationToken)
        {
            var countSpec = new CategoryCountSpec(null);
            var totalItems = await _repo.CountAsync(countSpec);

            var pagedSpec = new CategoryWithPagingSpec(null, request.PageIndex, request.PageSize);
            var categories = await _repo.GetAsync(pagedSpec);

            var mapped = _mapper.Map<IReadOnlyList<CategoryModel>>(categories);
            var result = new PagedResult<CategoryModel>(mapped, totalItems, request.PageIndex, request.PageSize);

            return Result.Success(result);
        }
    }
}
