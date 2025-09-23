using Ecommerce.Domain.Models;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Queries.Orders.GetOrderById
{
    public sealed record GetOrderByIdQuery(int orderId) : IRequest<Result<Order>>
    {
    }
}
