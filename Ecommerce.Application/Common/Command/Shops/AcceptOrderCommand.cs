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
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Shops
{
    public sealed record AcceptOrderCommand(string UserId, int OrderId) : IRequest<Result>
    {
    }
    public sealed class AcceptOrderHandler : IRequestHandler<AcceptOrderCommand, Result>
    {
        private readonly INotificationService _notificationService;
        private readonly IUnitOfWork unitOfWork;
        private readonly IOrderRepository _orderRepository;
        private readonly IShopRepository _shopRepository;
        public AcceptOrderHandler(INotificationService notificationService, IOrderRepository orderRepository, IShopRepository shopRepository, IUnitOfWork unitOfWork)
        {
            _notificationService = notificationService;
            _orderRepository = orderRepository;
            _shopRepository = shopRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(AcceptOrderCommand request, CancellationToken cancellationToken)
        {
            var shop = await _shopRepository.GetByUserIdAsync(request.UserId);
            
            if (shop == null || shop.UserId != request.UserId) return Result.Failure(new Error("404", "Đơn hàng này không thuộc cửa hàng của bạn"));
            if (!shop.IsActive) return Result.Failure(new Error("404", "Cửa hàng đã bị vô hiệu hóa"));

            var order = await _orderRepository.GetOrderByIdAsync(request.OrderId);
            if (order != null)
            {
                order.UpdateStatus(OrderStatus.Finished);

                var noti = Notification.Create("Đơn hàng của bạn đã được xác nhận", "", shop.UserId);
                await _orderRepository.Update(order);
                await _notificationService.SendNotificationAsync(order.CustomerId, noti);
                await unitOfWork.SaveChangesAsync(cancellationToken);
                return Result.Success();
            }
            return Result.Failure(new Error("404", "Không tìm thấy đơn hàng hoặc đơn hàng đã bị khách hủy"));
        }
    }
}