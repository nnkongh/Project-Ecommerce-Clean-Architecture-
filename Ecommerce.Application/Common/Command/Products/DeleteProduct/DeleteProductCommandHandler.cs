using AutoMapper;
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

namespace Ecommerce.Application.Common.Command.Products.DeleteProduct
{
    public sealed class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Result>
    {
        private readonly IProductRepository _productRepo;
        private readonly IShopRepository _shopRepository;
        private readonly IUnitOfWork _uow;

        public DeleteProductCommandHandler(IProductRepository productRepo, IUnitOfWork uow, IShopRepository shopRepository)
        {
            _productRepo = productRepo;
            _uow = uow;
            _shopRepository = shopRepository;
        }

        public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            if (request.id <= 0)
            {
                return Result.Failure(new Error("", "Id is not found"));
            }
            var product = await _productRepo.GetByIdAsync(request.id);
            if (product == null)
            {
                return Result.Failure(new Error("", "Item not found"));
            }
            var shop = await _shopRepository.GetByUserIdAsync(request.UserId);
            if (shop == null || product.ShopId != shop.Id)
            {
                return Result.Failure(new Error("", "Sản phẩm này không thuộc về cửa hàng của bạn"));
            }
            await _productRepo.Delete(product);
            await _uow.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
