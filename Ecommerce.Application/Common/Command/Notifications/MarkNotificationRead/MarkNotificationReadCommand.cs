using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Common.Command.Notifications.MarkNotificationRead
{
    public sealed record MarkNotificationReadCommand(string userId, int? notificationId) : IRequest<Result>
    {
    }
}
