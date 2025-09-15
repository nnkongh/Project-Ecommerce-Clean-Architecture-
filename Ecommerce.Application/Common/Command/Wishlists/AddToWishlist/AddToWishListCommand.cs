using Ecommerce.Application.DTOs.Models;
using Ecommerce.Application.DTOs.ModelsRequest.Wishlist;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Wishlists.AddToWishlist
{
    public sealed record AddToWishListCommand(AddToWishlistRequest Request) : IRequest<Result<WishlistModel>>
    {
    }
}
