using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Common.Queries.Category.GetChildCategoriesPaged
{
    public sealed record GetChildCategoriesPagedQuery(int ParentId, int PageIndex, int PageSize) : IRequest<Result<PagedResult<CategoryModel>>>
    {
    }
}
