using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Common.Queries.Category.GetRootCategoriesPaged
{
    public sealed record GetRootCategoriesPagedQuery(int PageIndex, int PageSize) : IRequest<Result<PagedResult<CategoryModel>>>
    {
    }
}
