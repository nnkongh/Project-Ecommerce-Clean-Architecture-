using Ecommerce.Application.Common.Command.Wishlists.AddToWishlist;
using Ecommerce.Application.Common.Command.Wishlists.MoveItemToCart;
using Ecommerce.Application.Common.Command.Wishlists.RemoveItemInWishlist;
using Ecommerce.Application.Common.Queries.Wishlist.GetWishlistById;
using Ecommerce.Application.Common.Queries.Wishlist.GetWishlistsByUserId;
using Ecommerce.Application.DTOs.ModelsRequest.Wishlist;
using Ecommerce.Domain.Models;
using Ecommerce.WebApi.Controllers.BaseController;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.WebApi.Controllers
{
    [Route("wishlist")]
    [Authorize]
    public class WishlistController : ApiController
    {
        public WishlistController(ISender sender) : base(sender)
        {
        }
        [HttpPost]
        [Route("add-to-wishlist")]
        public async Task<IActionResult> AddItemToWishlist(AddToWishlistRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(userId == null)
            {
                return Unauthorized();
            }
            var command = new AddToWishListCommand(request,userId);
            var result = await Sender.Send(command);
            return result.IsSuccess ? Ok(result.IsSuccess) : BadRequest(result.Error);
        }
        [HttpDelete]
        [Route("{wishlistId}/delete/{productId}")]
        public async Task<IActionResult> DeleteItemInWishlist(int productId, int wishlistId)
        {
            var command = new RemoveItemWishlistCommand(productId, wishlistId);
            var result = await Sender.Send(command);
            return result.IsSuccess ? Ok(result.IsSuccess) : BadRequest(result.Error);
        }
        [HttpGet]
        [Route("get-by-id/{wishlistId}")]
        public async Task<IActionResult> GetWishlistById(int wishlistId)
        {

            var query = new GetItemWishlistByIdQuery(wishlistId);
            var result = await Sender.Send(query);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
        [HttpGet]
        [Route("get-wishlists")]
        public async Task<IActionResult> GetWishlistsByUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            var query = new GetWishlistsByUserIdQuery(userId);
            var result = await Sender.Send(query);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
        [HttpPost]
        [Route("move-to-cart/{wishlistId}")]
        public async Task<IActionResult> MoveItemToCart(int wishlistId, [FromBody]MovetoCartRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            var command = new MoveItemToCartCommand(userId, wishlistId, request);
            var result = await Sender.Send(command);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
    }
}
