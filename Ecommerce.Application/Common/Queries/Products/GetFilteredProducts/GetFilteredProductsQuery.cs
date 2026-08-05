using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Common.Queries.Products.GetFilteredProducts
{
    public sealed record GetFilteredProductsQuery(
        string? SortBy,
        decimal? MinPrice,
        decimal? MaxPrice,
        int? CategoryId,
        string? SearchTerm
    ) : IRequest<Result<IReadOnlyList<ProductModel>>>;
}
