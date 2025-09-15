using AutoMapper;
using Ecommerce.Application.DTOs.CRUD.Cart;
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

namespace Ecommerce.Application.Common.Command.Carts.AddToCart
{
    public sealed class AddToCartCommandHandler : IRequestHandler<AddToCartCommand, Result>
    {
        private readonly IUnitOfWork _uow;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        public AddToCartCommandHandler(IUnitOfWork unitOfWork, ICartRepository cartRepository, IProductRepository productRepository)
        {
            _uow = unitOfWork;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public async Task<Result> Handle(AddToCartCommand handler, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(handler.request.productId);
            if (product == null)
            {
                return Result.Failure(new Error("ProductNotFound", "The specified product does not exist."));
            }
            var cart = await _cartRepository.GetCartByUserIdAsync(handler.request.userId);
            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = handler.request.userId,
                };
                await _cartRepository.AddAsync(cart);
            }
            cart.AddItem(handler.request.productId, handler.request.quantity, product.Price, product.Name);
            await _uow.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
