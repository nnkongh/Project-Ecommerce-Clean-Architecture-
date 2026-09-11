using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Common.Queries.Products.GetProductsByShopId
{
    public sealed record GetProductsByShopIdQuery(int ShopId) : IRequest<Result<IReadOnlyList<ProductModel>>>
    {
    }
}
