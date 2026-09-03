using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Common.Queries.Orders.GetOrdersByShop
{
    public sealed record GetOrdersByShopQuery(string UserId) : IRequest<Result<IReadOnlyList<OrderModel>>>
    {
    }
}
