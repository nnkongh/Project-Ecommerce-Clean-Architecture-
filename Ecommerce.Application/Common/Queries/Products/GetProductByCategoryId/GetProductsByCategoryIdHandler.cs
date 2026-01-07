using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.Base;
using Ecommerce.Domain.Shared;
using Ecommerce.Domain.Specification;
using Ecommerce.Domain.Specification.ProductSpec;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Queries.Products.GetProductByCategoryId
{
    public sealed class GetProductsByCategoryIdHandler : IRequestHandler<GetProductsByCategoryIdQuery, Result<IReadOnlyList<ProductModel>>>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public GetProductsByCategoryIdHandler(IProductRepository productRepository, IMapper mapper, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public async Task<Result<IReadOnlyList<ProductModel>>> Handle(GetProductsByCategoryIdQuery request, CancellationToken cancellationToken)
        {
            if(request.categoryId <= 0)
            {
                return Result.Failure<IReadOnlyList<ProductModel>>(Error.NullValue);
            }

            var category = await _categoryRepository.GetByIdAsync(request.categoryId);
            if(category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            var spec = new ProductWithCategorySpec(request.categoryId);

            var products = await _productRepository.GetAsync(spec);

            if(products == null || !products.Any())
            {
                return Result.Success<IReadOnlyList<ProductModel>>(Array.Empty<ProductModel>());
            }

            var mapped = _mapper.Map<IReadOnlyList<ProductModel>>(products);

            return Result.Success(mapped);
        }
    }
}
