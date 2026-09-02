using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Common.Queries.Notifications.GetUnreadNotificationCount
{
    public sealed class GetUnreadNotificationCountQueryHandler : IRequestHandler<GetUnreadNotificationCountQuery, Result<int>>
    {
        private readonly INotificationRepository _notificationRepo;

        public GetUnreadNotificationCountQueryHandler(INotificationRepository notificationRepo)
        {
            _notificationRepo = notificationRepo;
        }

        public async Task<Result<int>> Handle(GetUnreadNotificationCountQuery request, CancellationToken cancellationToken)
        {
            var count = await _notificationRepo.CountUnreadByUserIdAsync(request.userId);
            return Result.Success(count);
        }
    }
}
