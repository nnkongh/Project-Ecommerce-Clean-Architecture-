using AutoMapper;
using Ecommerce.Application.DTOs.Product;
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
            if (string.IsNullOrWhiteSpace(request.update.Name))
            {
                return Result.Failure<ProductModel>(new Error("", "Name can not be null"));
            }
            if (request.update.Price <= 0)
            {
                return Result.Failure<ProductModel>(new Error("", "Price must be greater than zero"));
            }
            var product = await _productRepo.GetByIdAsync(request.id);
            if (product == null)
            {
                return Result.Failure<ProductModel>(new Error("", $"Product with id {request.id} is not found"));
            }
            var entity = _mapper.Map<Product>(request.update);
            var updated = await _productRepo.UpdatePartialAsync(request.id, entity,
                                                    x => x.Name,
                                                    x => x.Price,
                                                    x => x.Description,
                                                    x => x.ImageUrl,
                                                    x => x.Stock);
            await _uow.SaveChangesAsync(cancellationToken);
            var mapped = _mapper.Map<ProductModel>(updated);
            return Result.Success(mapped);
        }
    }
}
