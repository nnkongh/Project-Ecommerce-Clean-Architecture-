using Ecommerce.Application.DTOs.CRUD;
using Ecommerce.Application.DTOs.Product;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Products.CreateProduct
{
    public record CreateProductCommand(CreateProductRequest create) : IRequest<ProductModel>
    {
    }
}
