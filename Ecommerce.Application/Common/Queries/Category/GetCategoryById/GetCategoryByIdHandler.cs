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
    public sealed class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, Result<IReadOnlyList<ProductModel>>>
    {
        private readonly ICategoryRepository _repo;
        private readonly IMapper _mapper;
        public GetCategoryByIdHandler(ICategoryRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }


        public async Task<Result<IReadOnlyList<ProductModel>>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.id <= 0)
            {
                return Result.Failure<IReadOnlyList<ProductModel>>(new Error("Null",$"{request.id} is null"));
            }
            var category = await _repo.GetByIdAsync(request.id);
            if (category is null)
            {
                return Result.Success<IReadOnlyList<ProductModel>>(Array.Empty<ProductModel>());
            }
            if (category.Products == null || !category.Products.Any())
            {
                // Category có tồn tại nhưng không có product
                return Result.Success<IReadOnlyList<ProductModel>>(Array.Empty<ProductModel>());
            }
            var mapped = _mapper.Map<IReadOnlyList<ProductModel>>(category.Products);
            return Result.Success(mapped);
        }
    }
}
