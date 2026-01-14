using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Shared;
using Ecommerce.Domain.Specification;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Queries.Category.GetCategoryById
{
    public sealed class GetChildCategoryHandler : IRequestHandler<GetChildCategoryQuery, Result<IReadOnlyList<CategoryModel>>>
    {
        private readonly ICategoryRepository _repo;
        private readonly IMapper _mapper;
        public GetChildCategoryHandler(ICategoryRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }


        public async Task<Result<IReadOnlyList<CategoryModel>>> Handle(GetChildCategoryQuery request, CancellationToken cancellationToken)
        {
            if (request.parentId <= 0)
            {
                return Result.Failure<IReadOnlyList<CategoryModel>>(new Error("Null",$"{request.parentId} is null"));
            }
            var parentCategory = await _repo.GetByIdAsync(request.parentId);
            if (parentCategory is null)
            {
                return Result.Success<IReadOnlyList<CategoryModel>>(Array.Empty<CategoryModel>());
            }
            var childCategories = await _repo.GetByAsync(c => c.ParentId == request.parentId);

            if (childCategories is null)
            {
                return Result.Success<IReadOnlyList<CategoryModel>>(
                    Array.Empty<CategoryModel>());
            }
            var mapped = _mapper.Map<IReadOnlyList<CategoryModel>>(childCategories);
            return Result.Success(mapped);
        }
    }
}
