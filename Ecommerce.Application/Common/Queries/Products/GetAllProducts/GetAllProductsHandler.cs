using AutoMapper;
using Ecommerce.Application.DTOs.Product;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Queries.Products.GetAllProducts
{
    public sealed class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, Result<IEnumerable<ProductModel>>>
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;
        public GetAllProductsHandler(IProductRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<ProductModel>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _repo.GetAllAsync();
            if (products is null || !products.Any())
            {
                return Result.Failure<IEnumerable<ProductModel>>(Error.NullValue);
            }
            var mapped = _mapper.Map<IEnumerable<ProductModel>>(products);
            return Result.Success(mapped);
        }
    }
}
