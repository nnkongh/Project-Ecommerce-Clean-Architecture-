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
            if (string.IsNullOrWhiteSpace(request.create.Name))
            {
                return Result.Failure<ProductModel>(new Error("", "Product name is required"));
            }
            if (request.create.Price <= 0)
            {
                return Result.Failure<ProductModel>(new Error("", "Product price must be greater than zero"));
            }
            var existing = await _categoryRepo.GetByIdAsync(request.create.CategoryId);
            if (existing == null)
            {
                return Result.Failure<ProductModel>(new Error("", $"The category with id {request.create.CategoryId} is not found"));
            }
            var entity = _mapper.Map<Product>(request.create);
            var item = await _productRepo.AddAsync(entity);
            var mapped = _mapper.Map<ProductModel>(item);
            await _uow.SaveChangesAsync(cancellationToken);
            return Result.Success(mapped);
        }
    }
}
