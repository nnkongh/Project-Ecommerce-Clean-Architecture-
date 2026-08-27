using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Domain.Models;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Products.CreateProduct
{
    public sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<ProductModel>>
    {
        private readonly IProductRepository _productRepo;
        private readonly IShopRepository _shopRepository; 
        private readonly ICategoryRepository _categoryRepo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public CreateProductCommandHandler(IProductRepository productRepo, IMapper mapper, IUnitOfWork uow, ICategoryRepository categoryRepo, IShopRepository shopRepository)
        {
            _productRepo = productRepo;
            _mapper = mapper;
            _uow = uow;
            _categoryRepo = categoryRepo;
            _shopRepository = shopRepository;
        }

        public async Task<Result<ProductModel>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var productRequest = request.create;
            if (string.IsNullOrWhiteSpace(productRequest.Name))
            {
                return Result.Failure<ProductModel>(new Error("", "Tên sản phẩm không được để trống"));
            }
            if (productRequest.Price <= 0)

            {
                return Result.Failure<ProductModel>(new Error("", "Giá sản phẩm phải lớn hơn 0"));
            }
            if (string.IsNullOrWhiteSpace(productRequest.ImageUrl))
            {
                return Result.Failure<ProductModel>(new Error("", "Vui lòng chọn hình ảnh cho sản phẩm"));
            }
            var existing = await _categoryRepo.GetByIdAsync(productRequest.ParentCategoryId);
            if (existing == null)
            {
                return Result.Failure<ProductModel>(new Error("", $"Danh mục không tồn tại"));
            }
            var shop = await _shopRepository.GetByUserIdAsync(request.userId);
            if (shop == null)
            {
                return Result.Failure<ProductModel>(new Error("", $"Chỉ có cửa hàng được tạo sản phẩm"));
            }
            var product = Product.Create(productRequest.Name, productRequest.ImageUrl,shop.Id, productRequest.Price,productRequest.Stock, productRequest.ParentCategoryId,productRequest.ChildCategoryId, productRequest.Description);
            var item = await _productRepo.AddAsync(product); 
            var mapped = _mapper.Map<ProductModel>(item);
            await _uow.SaveChangesAsync(cancellationToken);
            return Result.Success(mapped);
        }
    }
}
