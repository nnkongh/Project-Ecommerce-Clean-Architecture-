using AutoMapper;
using Ecommerce.Application.Common.Command.Orders.CreateOrder;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Application.DTOs.ModelsRequest.Order;
using Ecommerce.Domain.Enum;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Domain.Models;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Carts.CheckoutCart
{
    public sealed class CheckoutCartCommandHandler : IRequestHandler<CheckoutCartCommand, Result<OrderModel>>
    {
        private readonly ICartRepository _cartRepo;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _uow;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CheckoutCartCommandHandler(ICartRepository cartRepo, IMapper mapper, IMediator mediator, IUserRepository userRepository, IProductRepository productRepository, IUnitOfWork uow)
        {
            _cartRepo = cartRepo;
            _mapper = mapper;
            _mediator = mediator;
            _userRepository = userRepository;
            _productRepository = productRepository;
            _uow = uow;
        }

        public async Task<Result<OrderModel>> Handle(CheckoutCartCommand request, CancellationToken cancellationToken)
        {
            //Kiểm tra thông tin cơ bản của người dùng
            var userResult = await ValidatingUserInformation(request.userId);
            if (!userResult.IsSuccess)
                return Result.Failure<OrderModel>(userResult.Error);

            //Kiểm tra thông tin về số lượng sản phẩm của cart và tồn kho
            var cartResult = await ValidateCartItemAndStockProduct(userResult.Value.Id);
            if (!cartResult.IsSuccess)
                return Result.Failure<OrderModel>(cartResult.Error);

            var cartDto = _mapper.Map<CartModel>(cartResult.Value);
            var userDto = _mapper.Map<UserModel>(userResult.Value);

            var orderRequest = new CreateOrderRequest
            {
                User = userDto,
                Cart = cartDto,
            };

            var result = await _mediator.Send(new CreateOrderByCartCommand(orderRequest), cancellationToken);

            if (!result.IsSuccess)
            {
                return Result.Failure<OrderModel>(new Error("ORDER_CREATE_FAILED", "Không thể tạo đơn hàng"));
            }

            cartResult.Value.Clear();
            await _cartRepo.Delete(cartResult.Value);
            await _uow.SaveChangesAsync(cancellationToken);
            return Result.Success(result.Value);
        }
        private async Task<Result<User>> ValidatingUserInformation(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return Result.Failure<User>(new Error("USER_NOT_FOUND", "User không tồn tại"));

            if (user.Address == null)
                return Result.Failure<User>(new Error("ADDRESS_EMPTY", "User chưa có địa chỉ"));

            if (string.IsNullOrEmpty(user.Address.City) ||
                string.IsNullOrEmpty(user.Address.Street) ||
                string.IsNullOrEmpty(user.Address.Ward) ||
                string.IsNullOrEmpty(user.Address.District))
            {
                return Result.Failure<User>(new Error("ADDRESS_INVALID", "Địa chỉ chưa đầy đủ"));
            }

            return Result.Success(user);
        }

        private async Task<Result<Cart>> ValidateCartItemAndStockProduct(string userId)
        {
            var cart = await _cartRepo.GetCartWithItemByUserIdAsync(userId);
            if (cart == null)
                return Result.Failure<Cart>(new Error("CART_NOT_FOUND", "Cart không tồn tại"));

            if (cart.Items == null || cart.Items.Count == 0)
                return Result.Failure<Cart>(new Error("CART_EMPTY", "Giỏ hàng đang trống"));

            foreach (var item in cart.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);

                if (product == null)
                    return Result.Failure<Cart>(new Error("PRODUCT_NOT_FOUND",
                        $"Sản phẩm {item.ProductId} không tồn tại"));

                if (product.Stock < item.Quantity)
                    return Result.Failure<Cart>(new Error("OUT_OF_STOCK",
                        $"Sản phẩm {product.Name} không đủ tồn kho"));
            }

            return Result.Success(cart);
        }

    }
}
