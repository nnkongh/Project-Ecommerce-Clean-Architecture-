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
        private readonly ICategoryRepository _categoryRepo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public CreateProductCommandHandler(IProductRepository productRepo, IMapper mapper, IUnitOfWork uow, ICategoryRepository categoryRepo)
        {
            _productRepo = productRepo;
            _mapper = mapper;
            _uow = uow;
            _categoryRepo = categoryRepo;
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
            if (productRequest.ImageUrl == null)
            {
                return Result.Failure<ProductModel>(new Error("", "Hình ảnh sản phẩm không được để trống"));
            }
            var existing = await _categoryRepo.GetByIdAsync(productRequest.CategoryId);
            if (existing == null)
            {
                return Result.Failure<ProductModel>(new Error("", $"Danh mục không tồn tại"));
            }
            var product = Product.Create(productRequest.Name, productRequest.ImageUrl, productRequest.Price,productRequest.Stock, productRequest.CategoryId, productRequest.Description);
            var item = await _productRepo.AddAsync(product); 
            var mapped = _mapper.Map<ProductModel>(item);
            await _uow.SaveChangesAsync(cancellationToken);
            return Result.Success(mapped);
        }
    }
}
