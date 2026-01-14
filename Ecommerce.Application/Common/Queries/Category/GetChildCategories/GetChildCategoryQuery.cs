using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Queries.Category.GetCategoryById
{
    public sealed record GetChildCategoryQuery(int parentId) : IRequest<Result<IReadOnlyList<CategoryModel>>>
    {
    }
}
