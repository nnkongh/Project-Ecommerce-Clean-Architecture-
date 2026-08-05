using AutoMapper;
using Ecommerce.Application.Common.Queries.Category.GetAllCategories;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Shared;
using Ecommerce.Domain.Specification;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Queries.Category.GetRootDetailCategories
{
    public class GetRootDetailHandler : IRequestHandler<GetRootDetailQuery, Result<CategoryListPageResponse>>
    {
        // Có danh sách danh mục
        // Có sản phẩm thuộc danh mục gốc
        // Có phân trang sản phẩm
        private readonly IMapper _mapper;
        private readonly ISender _sender;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public GetRootDetailHandler(IMapper mapper, IProductRepository productRepository, ICategoryRepository categoryRepository, ISender sender)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _sender = sender;
        }

        public async Task<Result<CategoryListPageResponse>> Handle(GetRootDetailQuery request, CancellationToken cancellationToken)
        {
            var rootCategories = new GetRootCategoriesQuery();
            var categoryResult = await _sender.Send(rootCategories, cancellationToken);
            if (!categoryResult.IsSuccess)
            {
                return Result.Failure<CategoryListPageResponse>(new Error("",""));
            }
            var mappedCategories = _mapper.Map<IReadOnlyList<CategoryModel>>(categoryResult.Value);

            var countSpec = new ProductCountSpec();
            var totalItems = await _productRepository.CountAsync(countSpec);

            var pagedProductSpec = new ProductWithPagingSpec(request.pageIndex, request.pageSize);

            var productsResult = await _productRepository.GetAsync(pagedProductSpec);
            if(productsResult == null)
            {
                return Result.Failure<CategoryListPageResponse>(new Error("",""));
            }
            var mappedProducts = _mapper.Map<IReadOnlyList<ProductModel>>(productsResult);

            var response = new CategoryListPageResponse
            {
                Categories = mappedCategories,
                Products = new PagedResult<ProductModel>(mappedProducts, totalItems, request.pageIndex, request.pageSize)
            };

            return Result<CategoryListPageResponse>.Success(response);
        }
    }
}
