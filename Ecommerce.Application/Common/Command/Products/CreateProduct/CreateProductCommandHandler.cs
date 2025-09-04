using AutoMapper;
using Ecommerce.Application.DTOs.Product;
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
    public sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result>
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

        public async Task<Result> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            if(string.IsNullOrWhiteSpace(request.create.Name))
            {
                return Result.Failure(new Error("","Product name is required"));
            }
            if(request.create.Price <= 0){
                return Result.Failure(new Error("", "Product price must be greater than zero"));
            }
            var existing = await CheckIsExistsCategory(request.create.CategoryId);
            if (existing == false)
            {
                return Result.Failure(new Error("", $"The category with id {request.create.CategoryId} is not found"));
            }
            var entity = _mapper.Map<Product>(request.create);
            var item = await _productRepo.AddAsync(entity);
            var mapped = _mapper.Map<ProductModel>(item);
            await _uow.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        private async Task<bool> CheckIsExistsCategory(int categoryId)
        {
            var existing = await _categoryRepo.GetByIdASync(categoryId);
            if(existing == null)
            {
                return false;
            }
            return true;
        }
    }
}
