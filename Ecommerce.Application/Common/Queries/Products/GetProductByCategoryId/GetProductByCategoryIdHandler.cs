using AutoMapper;
using Ecommerce.Application.DTOs.Product;
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

namespace Ecommerce.Application.Products.Queries.Products.GetProductByCategory
{
    public sealed class GetProductByCategoryIdHandler : IRequestHandler<GetProductByCategoryIdQueries, Result<IEnumerable<ProductModel>>>
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;
        public GetProductByCategoryIdHandler(IProductRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<ProductModel>>> Handle(GetProductByCategoryIdQueries request, CancellationToken cancellationToken)
        {
            if(request.categoryId <= 0)
            {
                return Result.Failure<IEnumerable<ProductModel>>(Error.NullValue);
            }
            var list = await _repo.GetProductByCategoryIdAsync(request.categoryId);
            if(list is null || !list.Any())
            {
                return Result.Failure<IEnumerable<ProductModel>>(Error.NullValue);
            }
            var mapped = _mapper.Map<IEnumerable<ProductModel>>(list);
            return Result.Success(mapped);
        }
    }
}
