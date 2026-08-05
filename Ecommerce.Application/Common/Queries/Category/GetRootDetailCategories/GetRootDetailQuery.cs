using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Queries.Category.GetRootDetailCategories
{
    public sealed class GetRootDetailQuery : IRequest<Result<CategoryListPageResponse>>
    {
        public int pageIndex { get; set; }
        public int pageSize => 16;
    }
}
