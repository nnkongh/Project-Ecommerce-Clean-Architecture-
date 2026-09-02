using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Common.Queries.Notifications.GetUserNotifications
{
    public sealed record GetUserNotificationsQuery(string userId) : IRequest<Result<IReadOnlyList<NotificationModel>>>
    {
    }
}
