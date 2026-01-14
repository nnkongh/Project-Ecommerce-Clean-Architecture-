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

namespace Ecommerce.Application.Common.Queries.Products.GetProductsCategory
{
    public sealed class GetProductsByCategoryHandler : IRequestHandler<GetProductsByCategoryQuery, Result<IReadOnlyList<ProductModel>>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetProductsByCategoryHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<Result<IReadOnlyList<ProductModel>>> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
        {
            var spec = new ProductWithCategorySpec();
            
            var products = await _productRepository.GetAsync();

            if(products == null || products.Any())
            {
                return Result.Success<IReadOnlyList<ProductModel>>(Array.Empty<ProductModel>());
            }

            var mapped = _mapper.Map<IReadOnlyList<ProductModel>>(products);
            
            return Result.Success(mapped);
        }
    }
}
