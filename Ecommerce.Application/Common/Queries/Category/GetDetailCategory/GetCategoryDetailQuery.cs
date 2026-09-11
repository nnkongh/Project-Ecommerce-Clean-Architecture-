using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Queries.Category.GetDetailCategory
{
    public sealed record GetCategoryQuery(int? ParentCategoryId, int? SelectedCategoryId, int? page, int? pageSize) : IRequest<Result<CategoryDetailModel>>
    {
    }
}
