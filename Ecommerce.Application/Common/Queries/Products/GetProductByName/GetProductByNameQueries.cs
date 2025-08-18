using Ecommerce.Application.DTOs.Product;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Products.Queries.Products.GetProductByName
{
    public record GetProductByNameQueries(string name) : IRequest<IEnumerable<ProductModel>>
    {
    }
}
