using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Common.Queries.Products.GetProductsByShopId
{
    public sealed class GetProductsByShopIdHandler : IRequestHandler<GetProductsByShopIdQuery, Result<IReadOnlyList<ProductModel>>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetProductsByShopIdHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<Result<IReadOnlyList<ProductModel>>> Handle(GetProductsByShopIdQuery request, CancellationToken cancellationToken)
        {
            if (request.ShopId <= 0)
            {
                return Result.Failure<IReadOnlyList<ProductModel>>(Error.NullValue);
            }

            var products = await _productRepository.GetProductsByShopIdAsync(request.ShopId);

            if (products == null || !products.Any())
            {
                return Result.Success<IReadOnlyList<ProductModel>>(Array.Empty<ProductModel>());
            }

            var mapped = _mapper.Map<IReadOnlyList<ProductModel>>(products);

            return Result.Success(mapped);
        }
    }
}
