using Ecommerce.Application.DTOs.Models;
using Ecommerce.Application.DTOs.ModelsRequest.Carts;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Carts.AddToCart
{
    public sealed record AddToCartCommand(AddToCartRequest request, string userId) : IRequest<Result<CartModel>>
    {
    }
}
