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
            var cart = await _cartRepo.GetCartWithItemByUserIdAsync(request.userId);
            if (cart == null)
            {
                return Result.Failure(new Error("", $"Cart not found."));
            }
            
            var product = await _productRepo.GetByIdAsync(request.productId);
            if (product == null)
            {
                return Result.Failure(new Error("", "Product not found"));
            }
            var existingItem = cart.Items.Count(x => x.ProductId == request.productId);
            if (existingItem == 0) 
            {
                return Result.Failure(new Error("", $"Can not delete because your cart does not contain item {product.Name}"));
            }
            cart.RemoveItem(request.productId);
            product.Stock += existingItem;
            await _uow.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
