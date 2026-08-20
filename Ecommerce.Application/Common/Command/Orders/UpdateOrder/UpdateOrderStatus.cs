using Ecommerce.Application.Interfaces;
using Ecommerce.Domain;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Orders.UpdateOrder
{
    public sealed record UpdateOrderStatus(string UserId, int OrderId) : IRequest<Result>
    {
    }
    internal sealed class UpdateOrderStatusHandler : IRequestHandler<UpdateOrderStatus, Result>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notification;

        public UpdateOrderStatusHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork, INotificationService notification)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _notification = notification;
        }

        public async Task<Result> Handle(UpdateOrderStatus request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetOrderByIdAsync(request.OrderId);
            if (order == null)
            {
                return Result.Failure(new Error("404", "Không tìm thấy sản phẩm"));
            }
            if (order.OrderStatus != Domain.Enum.OrderStatus.Pending)
            {
                return Result.Failure(new Error("400", "Chỉ có thể cập nhật sản phẩm với trạng thái pending"));
            }
            if (order.CustomerId != request.UserId)
            {
                return Result.Failure(new Error("403", "Không thể xác nhận sản phẩm của người khác"));
            }
            order.UpdateStatus(Domain.Enum.OrderStatus.Processing);
            await _orderRepository.Update(order);

            var noti = Notification.Create("Xác nhận đơn", "Đơn hàng của bạn đã được cửa hàng xác nhận",order.CustomerId);
            await _notification.SendNotificationAsync(order.CustomerId, noti);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            return Result.Success();
        }
    }
}