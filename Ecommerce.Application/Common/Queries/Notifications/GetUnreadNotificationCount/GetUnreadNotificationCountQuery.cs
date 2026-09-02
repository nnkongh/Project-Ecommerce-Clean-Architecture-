using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Common.Queries.Notifications.GetUnreadNotificationCount
{
    public sealed record GetUnreadNotificationCountQuery(string userId) : IRequest<Result<int>>
    {
    }
}
