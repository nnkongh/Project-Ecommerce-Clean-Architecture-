using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Carts.RemoveItemInCart
{
    public sealed record RemoveItemCartCommand(int cartId, int productId, int quantity) : IRequest<Result>
    {
    }
}
