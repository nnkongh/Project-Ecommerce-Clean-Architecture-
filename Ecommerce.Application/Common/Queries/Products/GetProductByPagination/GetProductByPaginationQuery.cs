using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Common.Queries.Products.GetProductByPagination
{
    public sealed record GetProductByPaginationQuery(
        int page,
        int pageSize,
        string? SortBy = null,
        decimal? MinPrice = null,
        decimal? MaxPrice = null,
        int? CategoryId = null,
        string? SearchTerm = null
    ) : IRequest<PagedResult<ProductModel>>;
}