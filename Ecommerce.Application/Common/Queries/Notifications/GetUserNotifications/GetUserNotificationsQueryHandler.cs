using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Common.Queries.Notifications.GetUserNotifications
{
    public sealed class GetUserNotificationsQueryHandler : IRequestHandler<GetUserNotificationsQuery, Result<IReadOnlyList<NotificationModel>>>
    {
        private readonly INotificationRepository _notificationRepo;
        private readonly IMapper _mapper;

        public GetUserNotificationsQueryHandler(INotificationRepository notificationRepo, IMapper mapper)
        {
            _notificationRepo = notificationRepo;
            _mapper = mapper;
        }

        public async Task<Result<IReadOnlyList<NotificationModel>>> Handle(GetUserNotificationsQuery request, CancellationToken cancellationToken)
        {
            var notifications = await _notificationRepo.GetByUserIdAsync(request.userId);
            var mapped = _mapper.Map<IReadOnlyList<NotificationModel>>(notifications ?? new List<Ecommerce.Domain.Models.Notification>());
            return Result.Success(mapped);
        }
    }
}
