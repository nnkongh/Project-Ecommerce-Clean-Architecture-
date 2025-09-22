using Ecommerce.Application.Common.Command.Wishlists.AddToWishlist;
using Ecommerce.Application.Common.Command.Wishlists.RemoveItemInWishlist;
using Ecommerce.Application.Common.Queries.Wishlist.GetItemWishlist;
using Ecommerce.Application.DTOs.ModelsRequest.Wishlist;
using Ecommerce.Domain.Models;
using Ecommerce.WebApi.Controllers.BaseController;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.WebApi.Controllers
{
    [Route("Wishlist")]
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
            var command = new AddToWishListCommand(request);
            var result = await Sender.Send(command);
            return result.IsSuccess ? Ok(result.IsSuccess) : BadRequest(result.Error);
        }
        [HttpDelete]
        [Route("delete/{productId}/{userId}")]
        public async Task<IActionResult> DeleteItemInWishlist(int productId)
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (user == null)
            {
                return Unauthorized();
            }
            var command = new RemoveItemWishlistCommand(productId, user);
            var result = await Sender.Send(command);
            return result.IsSuccess ? Ok(result.IsSuccess) : BadRequest(result.Error);
        }
        [HttpGet]
        [Route("get-by-id/{wishlistId}")]
        public async Task<IActionResult> GetItemWishlistById(int wishlistId)
        {
            var query = new GetItemWishlistByIdQuery(wishlistId);
            var result = await Sender.Send(query);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
    }
}
