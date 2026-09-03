using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Common.Command.Notifications.MarkNotificationRead
{
    public sealed class MarkNotificationReadCommandHandler : IRequestHandler<MarkNotificationReadCommand, Result>
    {
        private readonly INotificationRepository _notificationRepo;
        private readonly IUnitOfWork _uow;

        public MarkNotificationReadCommandHandler(INotificationRepository notificationRepo, IUnitOfWork uow)
        {
            _notificationRepo = notificationRepo;
            _uow = uow;
        }

        public async Task<Result> Handle(MarkNotificationReadCommand request, CancellationToken cancellationToken)
        {
            if (request.notificationId.HasValue)
            {
                await _notificationRepo.MarkAsReadAsync(request.notificationId.Value, request.userId);
            }
            else
            {
                await _notificationRepo.MarkAllAsReadAsync(request.userId);
            }
                
            await _uow.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
