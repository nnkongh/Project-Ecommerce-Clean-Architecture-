using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Shared;
using Ecommerce.Domain.Specification;
using MediatR;

namespace Ecommerce.Application.Common.Queries.Products.GetProductByPagination
{
    public sealed class GetProductByPaginationHandler : IRequestHandler<GetProductByPaginationQuery, PagedResult<ProductModel>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public GetProductByPaginationHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<ProductModel>> Handle(GetProductByPaginationQuery request, CancellationToken cancellationToken)
        {
            var countSpec = new ProductFilterCountSpec(
                request.MinPrice, request.MaxPrice,
                request.CategoryId, request.SearchTerm);

            var spec = new ProductFilterWithPagingSpec(
                request.SortBy, request.MinPrice, request.MaxPrice,
                request.CategoryId, request.SearchTerm,
                request.page, request.pageSize);

            var totalItems = await _productRepository.CountAsync(countSpec);

            var items = await _productRepository.GetAsync(spec);

            var mappedItems = _mapper.Map<IReadOnlyList<ProductModel>>(items);

            return new PagedResult<ProductModel>(mappedItems, totalItems, request.page, request.pageSize);
        }
    }
}