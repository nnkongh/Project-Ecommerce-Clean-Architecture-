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
    public class GetCategoryDetailQuery : IRequest<Result<PagedResult<CategoryDetailModel>>>
    {
        public int? SelectedCategoryId { get; set; }
        public int? ParentCategoryId { get; set; }
    }
}
