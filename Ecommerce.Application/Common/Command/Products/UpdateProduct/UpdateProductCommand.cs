using Ecommerce.Application.DTOs.CRUD.Product;
using Ecommerce.Application.DTOs.Product;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Products.UpdateProduct
{
    public sealed record UpdateProductCommand(int id, UpdateProductRequest update, params Expression<Func<Task, object>>[] productProperties ) : IRequest<Result<ProductModel>>
    {
    }
}
