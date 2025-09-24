using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Carts.DeleteCart
{
    public sealed record DeleteCartCommand(string userId) : IRequest<Result>
    {
    }
}
