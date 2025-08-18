
using Ecommerce.Application.DTOs.Product;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Products.Queries.Products.GetProductByCategory
{
    public record GetProductByCategoryQueries(int categoryId) : IRequest<IEnumerable<ProductModel>>
    {
    }
}
