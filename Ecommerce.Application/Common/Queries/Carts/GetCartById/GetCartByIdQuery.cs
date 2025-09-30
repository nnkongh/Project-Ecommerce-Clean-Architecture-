using Ecommerce.Application.DTOs.Models;
using Ecommerce.Application.DTOs.Product;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Queries.Carts.GetCart
{
    public sealed record GetCartByIdQuery(int id) : IRequest<Result<IReadOnlyList<CartItemModel>>>
    {
    }
}
