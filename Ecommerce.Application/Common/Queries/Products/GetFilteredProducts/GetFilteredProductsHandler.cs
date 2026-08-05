using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Shared;
using Ecommerce.Domain.Specification;
using MediatR;

namespace Ecommerce.Application.Common.Queries.Products.GetFilteredProducts
{
    internal sealed class GetFilteredProductsHandler : IRequestHandler<GetFilteredProductsQuery, Result<IReadOnlyList<ProductModel>>>
    {
        private readonly IProductRepository _productRepo;
        private readonly IMapper _mapper;

        public GetFilteredProductsHandler(IProductRepository productRepo, IMapper mapper)
        {
            _productRepo = productRepo;
            _mapper = mapper;
        }

        public async Task<Result<IReadOnlyList<ProductModel>>> Handle(GetFilteredProductsQuery request, CancellationToken cancellationToken)
        {
            var spec = new ProductFilterSpec(
                request.SortBy,
                request.MinPrice,
                request.MaxPrice,
                request.CategoryId,
                request.SearchTerm);

            var products = await _productRepo.GetAsync(spec);
            var mapped = _mapper.Map<IReadOnlyList<ProductModel>>(products);
            return Result.Success(mapped);
        }
    }
}
