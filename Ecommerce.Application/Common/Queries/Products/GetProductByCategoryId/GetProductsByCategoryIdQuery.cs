using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Queries.Products.GetProductByCategoryId
{
    public sealed record GetProductsByCategoryIdQuery(int categoryId) : IRequest<Result<IReadOnlyList<ProductModel>>>
    {
    }
}
