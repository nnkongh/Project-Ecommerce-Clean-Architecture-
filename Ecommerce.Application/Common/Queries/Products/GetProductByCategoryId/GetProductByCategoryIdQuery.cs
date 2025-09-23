
using Ecommerce.Application.DTOs.Product;
using Ecommerce.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Products.Queries.Products.GetProductByCategory
{
    public sealed record GetProductByCategoryIdQuery(int categoryId) : IRequest<Result<IEnumerable<ProductModel>>>
    {
    }
}
