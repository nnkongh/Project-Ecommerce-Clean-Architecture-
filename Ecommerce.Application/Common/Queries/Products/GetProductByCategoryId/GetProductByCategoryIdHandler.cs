using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.Base;
using Ecommerce.Domain.Shared;
using Ecommerce.Domain.Specification;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Queries.Products.GetProductByCategoryId
{
    public sealed class GetProductByCategoryIdHandler : IRequestHandler<GetProductByCategoryIdQuery, Result<IEnumerable<ProductModel>>>
    {
        private readonly IProductRepository _productRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly IMapper _mapper;
        public GetProductByCategoryIdHandler(IProductRepository productRepo, IMapper mapper, ICategoryRepository categoryRepo)
        {
            _productRepo = productRepo;
            _mapper = mapper;
            _categoryRepo = categoryRepo;
        }

        public async Task<Result<IEnumerable<ProductModel>>> Handle(GetProductByCategoryIdQuery request, CancellationToken cancellationToken)
        {
            if(request.categoryId <= 0)
            {
                return Result.Failure<IEnumerable<ProductModel>>(Error.NullValue);
            }
            var existing = await CheckIsExistsCategory(request.categoryId);
            if (existing == false)
            {
                return Result.Failure<IEnumerable<ProductModel>>(new Error("", $"Category with id {request.categoryId} is not found"));
            }
            var list = await _productRepo.GetProductByCategoryIdAsync(request.categoryId);
            if(list is null || !list.Any())
            {
                return Result.Success(Enumerable.Empty<ProductModel>());
            }
            var mapped = _mapper.Map<IEnumerable<ProductModel>>(list);
            return Result.Success(mapped);
        }
        private async Task<bool> CheckIsExistsCategory(int categoryId)
        {
            var existing = await _categoryRepo.GetByIdAsync(categoryId);
            if (existing == null)
            {
                return false;
            }
            return true;
        }
    }
}
