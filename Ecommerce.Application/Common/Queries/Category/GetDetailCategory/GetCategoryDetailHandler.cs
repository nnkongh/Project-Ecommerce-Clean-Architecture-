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

namespace Ecommerce.Application.Common.Queries.Category.GetDetailCategory
{
    public class GetCategoryDetailHandler : IRequestHandler<GetCategoryDetailQuery, Result<PagedResult<CategoryDetailModel>>>
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

        public async Task<Result<PagedResult<CategoryDetailModel>>> Handle(GetCategoryDetailQuery request, CancellationToken cancellationToken)
        {
            // Lấy danh mục con
            var childCategory = await _categoryRepository.GetByAsync(c => c.ParentId == request.ParentCategoryId);
            if (childCategory == null || childCategory.Count == 0)
            {
                return Result.Failure<PagedResult<CategoryDetailModel>>(new Error("404", "Không tìm thấy danh mục con"));
            }
            // Lấy danh sách id của các danh mục con
            var categoryId = childCategory.Select(c => c.Id).ToList();

            var allProducts = new ProductWithCategorySpec(categoryId);

            var displayProducts = await _productRepository.GetAsync(allProducts);

            // Nhóm sản phẩm theo danh mục
            var productsByCategory = displayProducts.GroupBy(p => p.CategoryId).ToDictionary(g => g.Key, g => g.ToList());

            // 
            var categoriesWithProducts = childCategory.Select(c => new CategoryWithProductModel
            {
                Id = c.Id,
                Name = c.Name,
                Products = productsByCategory.ContainsKey(c.Id) ? _mapper.Map<IReadOnlyList<ProductModel>>(productsByCategory[c.Id])
                                                                : new List<ProductModel>()
            }).ToList();

            var displayProductModels = request.SelectedCategoryId.HasValue
                ? categoriesWithProducts.FirstOrDefault(c => c.Id == request.SelectedCategoryId.Value)?.Products
                    ?? new List<ProductModel>()
                : categoriesWithProducts.SelectMany(c => c.Products).ToList();

            var result = new CategoryDetailModel
            {
                ParentCategoryId = request.ParentCategoryId,
                SelectedCategoryId = request.SelectedCategoryId,
                ChildCategories = categoriesWithProducts,
                DisplayProducts = new PagedResult<ProductModel>(displayProductModels, displayProductModels.Count, 1, displayProductModels.Count),
            };
            var pagedResult = new PagedResult<CategoryDetailModel>(new List<CategoryDetailModel> { result }, 1, 1, 1);

            return Result.Success(pagedResult);
        }
    }
}
