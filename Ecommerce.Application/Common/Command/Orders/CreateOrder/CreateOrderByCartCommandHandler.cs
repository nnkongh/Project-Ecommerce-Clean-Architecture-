using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Enum;
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

namespace Ecommerce.Application.Common.Command.Orders.CreateOrder
{
    public sealed class CreateOrderByCartCommandHandler : IRequestHandler<CreateOrderByCartCommand, Result<OrderModel>>
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IProductRepository _productRepo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        public CreateOrderByCartCommandHandler(IOrderRepository orderRepo, IProductRepository productRepo, IMapper mapper, IUnitOfWork uow)
        {
            _orderRepo = orderRepo;
            _productRepo = productRepo;
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<Result<OrderModel>> Handle(CreateOrderByCartCommand request, CancellationToken cancellationToken)
        {
            var userInfo = request.order.User.Address;

            var address = Ecommerce.Domain.Models.Address.Create(userInfo!.District!,
                                              userInfo!.City!,
                                              userInfo!.Province,
                                              userInfo!.Street!,
                                              userInfo!.Ward!);

            var user = request.order.User;
            var order = Order.CreateOrder(user.Id, user.UserName, user.PhoneNumber, user.Email, address);

            var groupedItems = new Dictionary<int, (string ShopName, List<Application.DTOs.Models.CartItemModel> Items)>();

            foreach (var item in request.order.Cart.Items)
            {
                var product = await _productRepo.GetByIdAsync(item.ProductId);
                var shopId = product?.ShopId ?? 0;

                if (!groupedItems.ContainsKey(shopId))
                {
                    groupedItems[shopId] = (ShopName: product?.Shop?.Name ?? "Unknown", new List<Application.DTOs.Models.CartItemModel>());
                }
                groupedItems[shopId].Items.Add(item);
            }

            foreach (var (shopId, group) in groupedItems)
            {
                var subOrder = SubOrder.Create(order.Id, shopId, group.ShopName);
                foreach (var item in group.Items)
                {
                    subOrder.AddItem(item.ImageUrl ?? "", item.ProductName ?? "", item.ProductId, item.UnitPrice, item.Quantity);
                }
                order.AddSubOrder(subOrder);
            }

            await _orderRepo.AddAsync(order);
            await _uow.SaveChangesAsync(cancellationToken);
            var mapped = _mapper.Map<OrderModel>(order);
            return Result.Success(mapped);
            
        }
    }
}
