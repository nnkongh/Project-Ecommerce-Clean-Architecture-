using Ecommerce.Application.DTOs.Product;
using Ecommerce.Domain.DTOs.Product;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Orders.CreateOrder
{
    public sealed record CreateOrderCommand(CartModel cart) : IRequest<Result<OrderModel>>
    {
    }
}
