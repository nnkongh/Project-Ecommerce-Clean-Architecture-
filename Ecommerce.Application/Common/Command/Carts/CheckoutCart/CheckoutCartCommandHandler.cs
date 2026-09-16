using AutoMapper;
using Ecommerce.Application.Common.Command.Orders.CreateOrder;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Application.DTOs.ModelsRequest.Order;
using Ecommerce.Application.Interfaces;
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
        private readonly ICartRepository _cartRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IShopRepository _shopRepository;
        private readonly INotificationService _notificationService;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CheckoutCartCommandHandler(ICartRepository cartRepository, IMapper mapper, IUserRepository userRepository, IProductRepository productRepository, IShopRepository shopRepository, INotificationService notificationService, IUnitOfWork uow, IOrderRepository orderRepository)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _productRepository = productRepository;
            _shopRepository = shopRepository;
            _notificationService = notificationService;
            _uow = uow;
            _orderRepository = orderRepository;
        }

        public async Task<Result<OrderModel>> Handle(CheckoutCartCommand request, CancellationToken cancellationToken)
        {
            var userResult = await ValidatingUserInformation(request.userId);
            if (!userResult.IsSuccess)
                return Result.Failure<OrderModel>(userResult.Error);

            var u = userResult.Value;

            var cart = await _cartRepository.GetCartWithItemByUserIdAsync(u.Id);
            if (cart == null)
                return Result.Failure<OrderModel>(new Error("CART_NOT_FOUND", "Cart không tồn tại"));

            var order = Order.CreateOrder(u.Id, u.UserName!, u.PhoneNumber, u.Email, u.Address!);
            var notifiedShopIds = new HashSet<int>();

            var productIds = cart.Items.Select(x => x.ProductId).ToList();
            var products = await _productRepository.GetProductsByIdsAsync(productIds);
            var productDict = products.ToDictionary(p => p.Id);


            foreach (var item in cart.Items)
            {
                if (!productDict.TryGetValue(item.ProductId, out var product))
                    return Result.Failure<OrderModel>(new Error("PRODUCT_NOT_FOUND", $"Sản phẩm {item.ProductId} không tồn tại"));

                if (product.Stock < item.Quantity)
                    return Result.Failure<OrderModel>(new Error("OUT_OF_STOCK", $"Sản phẩm {product.Name} không đủ tồn kho"));
            }

            var shopIds = products.Where(p => p.ShopId.HasValue)
                                  .Select(p => p.ShopId!.Value)
                                  .Distinct()
                                  .ToList();

            var shops = await _shopRepository.GetByIdsAsync(shopIds);
            var shopDict = shops.ToDictionary(p => p.Id);


            foreach (var item in cart.Items)
            {
                if (!productDict.TryGetValue(item.ProductId, out var product ) || product.ShopId is null) continue;

                if (!shopDict.TryGetValue(product.ShopId.Value, out var shop)) continue;

                var subOrder = SubOrder.Create(order.Id, product.ShopId.Value, shop.Name);
                subOrder.AddItem(product.ImageUrl,product.Name,product.Id,product.Price,item.Quantity);
                order.AddSubOrder(subOrder);
                product.AdjustStock(-item.Quantity);
                notifiedShopIds.Add(product.ShopId.Value);

            }

            var mapped = _mapper.Map<OrderModel>(order);

            cart.Clear();
            await _orderRepository.AddAsync(order);
            await _cartRepository.Delete(cart);
            await _uow.SaveChangesAsync(cancellationToken);
            foreach (var notiId in notifiedShopIds)
            {
                var shop = shopDict[notiId];
                var noti = Notification.Create("Đơn hàng mới", $"Bạn có đơn hàng mới #{order.Id} từ khách hàng {u.UserName}", shop.UserId, order.Id);
                await _notificationService.SendNotificationAsync(noti, cancellationToken);
            }
                
            return Result.Success(mapped);
        }
        private async Task<Result<User>> ValidatingUserInformation(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return Result.Failure<User>(new Error("USER_NOT_FOUND", "User không tồn tại"));

            if (user.PhoneNumber == null)
                return Result.Failure<User>(new Error("PHONE_NUMBER_EMPTY", "User chưa có số điện thoại"));

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
    }
}
