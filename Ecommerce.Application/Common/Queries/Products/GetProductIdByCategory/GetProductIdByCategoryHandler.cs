using AutoMapper;
using Ecommerce.Application.DTOs.Product;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Shared;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Queries.Products.GetProductIdByCategory
{
    public sealed class GetProductIdByCategoryHandler : IRequestHandler<GetProductIdByCategoryQuery, Result<ProductModel>>
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;

        public GetProductIdByCategoryHandler(IMapper mapper, IProductRepository repo)
        {
            _mapper = mapper;
            _repo = repo;
        }

        public async Task<Result<ProductModel>> Handle(GetProductIdByCategoryQuery request, CancellationToken cancellationToken)
        {
            if(request.productId <= 0)
            {
                return Result.Failure<ProductModel>(Error.NullValue);
            }
            var item = await _repo.GetProductByIdWithCategoryAsync(request!.productId);
            if(item == null)
            {
                return Result.Failure<ProductModel>(Error.NullValue);
            }
            var mapped = _mapper.Map<ProductModel>(item);
            return Result.Success(mapped);
        }
    }
}
