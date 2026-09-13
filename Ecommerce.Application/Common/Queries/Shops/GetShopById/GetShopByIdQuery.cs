using Ecommerce.Application.Common.Command.Shops;
using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Common.Queries.Shops.GetShopById
{
    public sealed record GetShopByIdQuery(int Id) : IRequest<Result<ShopModel>>
    {
    }
}