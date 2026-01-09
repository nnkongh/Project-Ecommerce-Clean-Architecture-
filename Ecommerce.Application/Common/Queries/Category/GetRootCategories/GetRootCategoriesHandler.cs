using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Queries.Category.GetAllCategories
{
    public sealed class GetRootCategoriesHandler : IRequestHandler<GetRootCategoriesQuery, Result<IReadOnlyList<CategoryModel>>>
    {
        private readonly ICategoryRepository _repo;
        private readonly IMapper _mapper;

        public GetRootCategoriesHandler(IMapper mapper, ICategoryRepository repo)
        {
            _mapper = mapper;
            _repo = repo;
        }

        public async Task<Result<IReadOnlyList<CategoryModel>>> Handle(GetRootCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _repo.GetByAsync(x => x.ParentId == null);

            if(!categories.Any())
            {
                return Result.Success<IReadOnlyList<CategoryModel>>(Array.Empty<CategoryModel>());
            }
            var mapped = _mapper.Map<IReadOnlyList<CategoryModel>>(categories);
            return Result.Success(mapped);
        }
    }
}
