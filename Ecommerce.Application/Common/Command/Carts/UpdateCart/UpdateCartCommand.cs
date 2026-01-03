using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Carts.UpdateCart
{
    public sealed record UpdateCartCommand(int productId, int quantity, string userId) : IRequest<Result<CartModel>>
    {
    }
}
