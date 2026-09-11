using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Queries.Category.GetDetailCategory
{
    public sealed class GetCategoryDetailHandler : IRequestHandler<GetCategoryQuery, Result<CategoryDetailModel>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetCategoryDetailHandler(ICategoryRepository categoryRepository, IMapper mapper, IProductRepository productRepository)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _productRepository = productRepository;
        }

        public async Task<Result<CategoryDetailModel>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            if (!request.ParentCategoryId.HasValue)
            {
                return Result.Failure<CategoryDetailModel>(new Error("InvalidCategory", "ParentCategoryId is required"));
            }

            var parentCategoryId = request.ParentCategoryId.Value;

            var childCategories = await _categoryRepository.GetChildCategoriesAsync(parentCategoryId);

            var childCategoryModels = new List<CategoryWithProductModel>();
            foreach (var child in childCategories)
            {
                var childProducts = await _productRepository.GetProductsByCategoryIdAsync(child.Id);
                childCategoryModels.Add(new CategoryWithProductModel
                {
                    Id = child.Id,
                    Name = child.Name,
                    Products = _mapper.Map<IReadOnlyList<ProductModel>>(childProducts)
                });
            }

            var displayCategoryId = request.SelectedCategoryId ?? parentCategoryId;
            var products = await _productRepository.GetProductsByCategoryIdAsync(displayCategoryId);
            var count = products.Count();
            var productDto = _mapper.Map<IReadOnlyList<ProductModel>>(products);

            var page = request.page ?? 1;
            var pageSize = request.pageSize ?? 12;
            var items = productDto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var pagedProduct = new PagedResult<ProductModel>(items, count, page, pageSize);

            var category = new CategoryDetailModel
            {
                ParentCategoryId = parentCategoryId,
                SelectedCategoryId = request.SelectedCategoryId,
                ChildCategories = childCategoryModels,
                DisplayProducts = pagedProduct
            };

            return Result.Success(category);
        }
    }
}