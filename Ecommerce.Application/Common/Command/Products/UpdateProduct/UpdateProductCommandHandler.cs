using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.Base;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Domain.Models;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Products.UpdateProduct
{
    public sealed class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result<ProductModel>>
    {
        private readonly IProductRepository _productRepo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public UpdateProductCommandHandler(IMapper mapper, IProductRepository productRepo, IUnitOfWork uow)
        {
            _mapper = mapper;
            _productRepo = productRepo;
            _uow = uow;
        }

        public async Task<Result<ProductModel>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            if ((request.update.Stock.HasValue && request.update.Stock < 0) ||
                (request.update.Price.HasValue && request.update.Price < 0))
            {
                return Result.Failure<ProductModel>(new Error("", $"Số lượng và giá không được phép âm"));
            }
            if (request.update.Name != null && string.IsNullOrWhiteSpace(request.update.Name))
            {
                return Result.Failure<ProductModel>(new Error("", "Tên không được rỗng"));
            }

            var product = await _productRepo.GetByIdAsync(request.id);

            if (product == null)
            {
                return Result.Failure<ProductModel>(new Error("", $"Product with id {request.id} is not found"));
            }

            product.Name = request.update.Name ?? product.Name;

            product.Price = request.update.Price ?? product.Price;

            product.Stock = request.update.Stock ?? product.Stock;

            product.ImageUrl = request.update.ImageUrl ?? product.ImageUrl;

            product.Description = request.update.Description ?? product.Description;

            //await _productRepo.Update(product);
            await _uow.SaveChangesAsync(cancellationToken);
            var mapped = _mapper.Map<ProductModel>(product);
            return Result.Success(mapped);
        }

    }

}
