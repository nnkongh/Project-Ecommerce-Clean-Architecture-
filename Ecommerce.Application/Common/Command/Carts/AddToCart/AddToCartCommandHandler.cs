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
        private readonly ICartRepository _cartRepo;
        private readonly IProductRepository _productRepo;
        private readonly IUserRepository _userRepo;
        public AddToCartCommandHandler(IUnitOfWork unitOfWork, ICartRepository cartRepo, IProductRepository productRepo, IUserRepository userRepo)
        {
            _uow = unitOfWork;
            _cartRepo = cartRepo;
            _productRepo = productRepo;
            _userRepo = userRepo;
        }

        public async Task<Result> Handle(AddToCartCommand handler, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetByIdAsync(handler.userId);
            if(user == null)
            {
                return Result.Failure(new Error("", "User not found"));
            }
            var product = await _productRepo.GetByIdAsync(handler.request.productId);
            if (product == null)
            {
                return Result.Failure(new Error("ProductNotFound", "The specified product does not exist."));
            }
            if(product.Stock < handler.request.quantity)
            {
                return Result.Failure(new Error("", $"Product {product.Name} has only {product.Stock} items left"));
            }
            var cart = await _cartRepo.GetCartWithItemByUserIdAsync(handler.userId);
            if (cart == null)
            {
                cart = Cart.CreateCart(handler.userId);
                await _cartRepo.AddAsync(cart);
            }
            cart.AddItem(handler.request.productId, handler.request.quantity, product.Price, product.Name);
            product.Stock -= handler.request.quantity;
            await _uow.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
