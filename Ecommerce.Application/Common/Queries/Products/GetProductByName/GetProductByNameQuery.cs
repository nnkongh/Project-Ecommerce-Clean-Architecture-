using Ecommerce.Application.DTOs.Product;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Products.Queries.Products.GetProductByName
{
    public sealed record GetProductByNameQuery(string name) : IRequest<Result<IEnumerable<ProductModel>>>
    {
    }
}
