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

namespace Ecommerce.Application.Common.Command.Carts.AddToCart
{
    public sealed class AddToCartCommandHandler : IRequestHandler<AddToCartCommand, Result<CartModel>>
    {
        private readonly IUnitOfWork _uow;
        private readonly ICartRepository _cartRepo;
        private readonly IProductRepository _productRepo;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        public AddToCartCommandHandler(IUnitOfWork unitOfWork, ICartRepository cartRepo, IProductRepository productRepo, IUserRepository userRepo, IMapper mapper)
        {
            _uow = unitOfWork;
            _cartRepo = cartRepo;
            _productRepo = productRepo;
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<Result<CartModel>> Handle(AddToCartCommand handler, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetByIdAsync(handler.userId);
            if(user == null)
            {
                return Result.Failure<CartModel>(new Error("", "User not found"));
            }
            var product = await _productRepo.GetByIdAsync(handler.request.Id);
            if (product == null)
            {
                return Result.Failure<CartModel>(new Error("ProductNotFound", "The specified product does not exist."));
            }
            if(product.Stock < handler.request.quantity)
            {
                return Result.Failure<CartModel>(new Error("", $"Product {product.Name} has only {product.Stock} items left"));
            }
            var cart = await _cartRepo.GetCartWithItemByUserIdAsync(handler.userId);
            if (cart == null)
            {
                cart = Cart.CreateCart(handler.userId);
                await _cartRepo.AddAsync(cart);
            }

            cart.AddItem(handler.request.Id, handler.request.quantity, product.Price, product.Name);
            await _uow.SaveChangesAsync(cancellationToken);

            var mapped = _mapper.Map<CartModel>(cart);
            return Result.Success(mapped);
        }
    }
}
