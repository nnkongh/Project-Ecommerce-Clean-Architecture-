using Ecommerce.Application.Common.Command.Shops;
using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Common.Queries.Shops.GetShopByUserId
{
    public sealed record GetShopByUserIdQuery(string UserId) : IRequest<Result<ShopModel>>
    {
    }
}
