using Ecommerce.Application.DTOs.Product;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Queries.Products.GetAllProducts
{
    public sealed record GetAllProductsQuery : IRequest<Result<IEnumerable<ProductModel>>>
    {
    }
}
