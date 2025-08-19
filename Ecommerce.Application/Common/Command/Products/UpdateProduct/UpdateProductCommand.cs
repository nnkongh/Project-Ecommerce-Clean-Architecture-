using Ecommerce.Application.DTOs.CRUD;
using Ecommerce.Application.DTOs.Product;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Products.UpdateProduct
{
    public record UpdateProductCommand(UpdateProductRequest update) : IRequest<Result>
    {
    }
}
