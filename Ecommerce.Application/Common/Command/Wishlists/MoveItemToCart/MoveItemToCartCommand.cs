using Ecommerce.Application.DTOs.Models;
using Ecommerce.Application.DTOs.ModelsRequest.Wishlist;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Wishlists.MoveItemToCart
{
    public sealed record MoveItemToCartCommand(string userId, int wishlistId, MovetoCartRequest request) : IRequest<Result<CartModel>>
    {
    }
}
