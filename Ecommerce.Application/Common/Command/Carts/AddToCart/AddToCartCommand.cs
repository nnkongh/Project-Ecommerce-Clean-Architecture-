using Ecommerce.Application.DTOs.CRUD.Cart;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Carts.AddToCart
{
    public sealed record AddToCartCommand(string UserId, int ProductId, int Quantity) : IRequest<Result<CreateCartRequest>>
    {
    }
}
