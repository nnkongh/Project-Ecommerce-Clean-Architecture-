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
    public sealed class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, Result<CategoryModel>>
    {
        private readonly ICategoryRepository _repo;
        private readonly IMapper _mapper;
        public GetCategoryByIdHandler(ICategoryRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }


        public async Task<Result<CategoryModel>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.id <= 0)
            {
                return Result.Failure<CategoryModel>(new Error("Null",$"{request.id} is null"));
            }
            var category = await _repo.GetByIdAsync(request.id);
            if (category is null)
            {
                return Result.Failure<CategoryModel>(Error.NullValue);
            }
            var mapped = _mapper.Map<CategoryModel>(category);
            return mapped;
        }
    }
}
