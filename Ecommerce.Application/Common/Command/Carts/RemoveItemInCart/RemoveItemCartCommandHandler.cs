using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Carts.RemoveItemInCart
{
    public sealed class RemoveItemCartCommandHandler : IRequestHandler<RemoveItemCartCommand, Result>
    {
        private readonly IUnitOfWork _uow;
        private readonly ICartRepository _cartRepo;
        private readonly IProductRepository _productRepo;
        public RemoveItemCartCommandHandler(IUnitOfWork unitOfWork, ICartRepository cartRepository, IProductRepository productRepository, IProductRepository productRepo)
        {
            _uow = unitOfWork;
            _cartRepo= cartRepository;
            _productRepo = productRepo;
        }

        public async Task<Result> Handle(RemoveItemCartCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.userId))
            {
                return Result.Failure(new Error("NotFound", "UserId must be provided."));
            }
            var cart = await _cartRepo.GetCartByUserIdAsync(request.userId);
            if (cart == null)
            {
                return Result.Failure(new Error("CartNotFound", "The specified cart does not exist."));
            }
            
            var product = await _productRepo.GetByIdAsync(request.productId);
            if (product == null)
            {
                return Result.Failure(new Error("", "Product not found"));
            }
            var existingItem = cart.Items.Any(x => x.ProductId == request.productId);
            if (!existingItem)
            {
                return Result.Failure(new Error("", $"Can not delete because your cart does not contain item {product.Name}"));
            }
            cart.RemoveItem(request.productId);
            product.Stock += request.quantity;
            await _uow.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
