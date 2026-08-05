using Ecommerce.Application.DTOs.Models;
using Ecommerce.Application.DTOs.ModelsRequest.Wishlist;
using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Common.Command.Wishlists.AddToWishlist
{
    public sealed record AddToWishListCommand(AddToWishlistRequest Request, string UserId) : IRequest<Result<WishlistModel>>
    {
    }
}
