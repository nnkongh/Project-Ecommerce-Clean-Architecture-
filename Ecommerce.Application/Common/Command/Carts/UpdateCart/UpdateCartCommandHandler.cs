using AutoMapper;
using Ecommerce.Application.DTOs.Product;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Carts.UpdateCart
{
    public sealed class UpdateCartCommandHandler : IRequestHandler<UpdateCartCommand, Result<CartModel>>
    {
        private readonly IUserRepository _userRepo;
        private readonly IProductRepository _productRepo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ICartRepository _cartRepo;
        public UpdateCartCommandHandler(IMapper mapper, IUnitOfWork uow, IProductRepository productRepo, IUserRepository userRepo, ICartRepository cartRepo)
        {
            _mapper = mapper;
            _uow = uow;
            _productRepo = productRepo;
            _userRepo = userRepo;
            _cartRepo = cartRepo;
        }

        public async Task<Result<CartModel>> Handle(UpdateCartCommand request, CancellationToken cancellationToken)
        {
            if(request.quantity <= 0)
            {
                return Result.Failure<CartModel>(new Error("", "Quantity can not be smaller than zero"));
            }
            var user = await _userRepo.GetByIdAsync(request.userId);
            if (user == null) {
                return Result.Failure<CartModel>(new Error("", "User not found"));
            }
            var product = await _productRepo.GetByIdAsync(request.productId);
            if (product == null)
            {
                return Result.Failure<CartModel>(new Error("", "Product not found"));
            }
            var cart = await _cartRepo.GetCartWithItemByUserIdAsync(user.Id);
            if (cart == null)
            {
                return Result.Failure<CartModel>(new Error("", "Cart not found"));
            }
            cart.ReduceItemQuantity(request.productId, request.quantity);
            product.Stock += request.quantity;
            await _uow.SaveChangesAsync(cancellationToken);
            var mapped = _mapper.Map<CartModel>(cart);
            return Result.Success(mapped);
        }
    }
}
