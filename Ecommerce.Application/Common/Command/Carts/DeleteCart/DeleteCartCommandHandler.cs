using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Carts.DeleteCart
{
    public sealed class DeleteCartCommandHandler : IRequestHandler<DeleteCartCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepo;

        public DeleteCartCommandHandler(ICartRepository cartRepository, IUnitOfWork unitOfWork, IProductRepository productRepo)
        {
            _cartRepository = cartRepository;
            _unitOfWork = unitOfWork;
            _productRepo = productRepo;
        }

        public async Task<Result> Handle(DeleteCartCommand request, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetCartWithItemByUserIdAsync(request.userId);
            if(cart == null)
            {
                return Result.Failure(new Error("CartNotFound", "The specified cart does not exist."));
            }
            var productIds = cart.Items.Select(i => i.ProductId).ToList();
            var products = await _productRepo.GetProductsByIdsAsync(productIds);
            var productMap = products.ToDictionary(p => p.Id);
            foreach (var item in cart.Items)
            {
                if(productMap.TryGetValue(item.ProductId, out var product))
                {
                    product.AdjustStock(item.Quantity);
                }
            }
            await _cartRepository.Delete(cart);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
