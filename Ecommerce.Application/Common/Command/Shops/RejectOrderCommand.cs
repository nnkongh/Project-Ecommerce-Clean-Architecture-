using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Enum;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Domain.Models;
using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Common.Command.Shops
{
    public sealed record RejectOrderCommand(string UserId, int OrderId) : IRequest<Result>
    {
    }

    public sealed class RejectOrderHandler : IRequestHandler<RejectOrderCommand, Result>
    {
        private readonly INotificationService _notificationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderRepository _orderRepository;
        private readonly IShopRepository _shopRepository;

        public RejectOrderHandler(INotificationService notificationService, IOrderRepository orderRepository, IShopRepository shopRepository, IUnitOfWork unitOfWork)
        {
            _notificationService = notificationService;
            _orderRepository = orderRepository;
            _shopRepository = shopRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(RejectOrderCommand request, CancellationToken cancellationToken)
        {
            var shop = await _shopRepository.GetByUserIdAsync(request.UserId);

            if (shop == null || shop.UserId != request.UserId)
                return Result.Failure(new Error("404", "Đơn hàng này không thuộc cửa hàng của bạn"));
            if (!shop.IsActive)
                return Result.Failure(new Error("404", "Cửa hàng đã bị vô hiệu hóa"));

            var order = await _orderRepository.GetOrderByIdAsync(request.OrderId);
            if (order == null)
                return Result.Failure(new Error("404", "Không tìm thấy đơn hàng"));

            if (order.OrderStatus != OrderStatus.Pending)
                return Result.Failure(new Error("400", "Chỉ có thể từ chối đơn hàng đang chờ xử lý"));

            order.UpdateStatus(OrderStatus.Rejected);

            var noti = Notification.Create("Đơn hàng đã bị từ chối", $"Đơn hàng #{order.Id} đã bị cửa hàng từ chối", order.CustomerId);
            await _orderRepository.Update(order);
            await _notificationService.SendNotificationAsync(order.CustomerId, noti);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
