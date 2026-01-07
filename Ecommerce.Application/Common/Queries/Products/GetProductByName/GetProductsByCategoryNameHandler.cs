using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Shared;
using Ecommerce.Domain.Specification;
using Ecommerce.Domain.Specification.ProductSpec;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Queries.Products.GetProductByName
{
    public sealed class GetProductsByCategoryNameHandler : IRequestHandler<GetProductByCategoryNameQuery, Result<IReadOnlyList<ProductModel>>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetProductsByCategoryNameHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }
        public async Task<Result<IReadOnlyList<ProductModel>>> Handle(GetProductByCategoryNameQuery request, CancellationToken cancellationToken)
        {
            var spec = new ProductWithCategorySpec(request.name);

            var produtcs = await _productRepository.GetAsync(spec);

            if(produtcs == null || !produtcs.Any())
            {
                return Result.Success<IReadOnlyList<ProductModel>>(Array.Empty<ProductModel>());
            }

            var mapped = _mapper.Map<IReadOnlyList<ProductModel>>(produtcs);

            return Result.Success(mapped);
        }
    }
}
