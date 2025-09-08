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
    public class RemoveItemCartCommandHandler : IRequestHandler<RemoveItemCartCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public RemoveItemCartCommandHandler(IUnitOfWork unitOfWork, ICartRepository cartRepository, IProductRepository productRepository)
        {
            _unitOfWork = unitOfWork;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public async Task<Result> Handle(RemoveItemCartCommand request, CancellationToken cancellationToken)
        {
            if(request.productId <= 0)
            {
                return Result.Failure(new Error("NotFound", "ProductId must be greater than zero."));
            }
            if(string.IsNullOrWhiteSpace(request.userId))
            {
                return Result.Failure(new Error("NotFound", "UserId must be provided."));
            }
            var product = await _productRepository.GetByIdAsync(request.productId);
            if (product == null)
            {
                return Result.Failure(new Error("ProductNotFound", "The specified product does not exist."));
            }
            var cart = await _cartRepository.GetCartByUserIdAsync(request.userId);
            if(cart == null)
            {
                return Result.Failure(new Error("CartNotFound", "The specified cart does not exist."));
            }
            cart.RemoveItem(request.productId);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
