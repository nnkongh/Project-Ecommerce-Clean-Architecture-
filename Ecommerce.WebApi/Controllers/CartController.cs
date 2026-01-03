using Ecommerce.Application.Common.Command.Carts.AddToCart;
using Ecommerce.Application.Common.Command.Carts.CheckoutCart;
using Ecommerce.Application.Common.Command.Carts.RemoveItemInCart;
using Ecommerce.Application.Common.Command.Carts.UpdateCart;
using Ecommerce.Application.Common.Queries.Carts.GetCartById;
using Ecommerce.Application.Common.Queries.Carts.GetCartByUserId;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Application.DTOs.ModelsRequest.Cart;
using Ecommerce.WebApi.Controllers.BaseController;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ecommerce.WebApi.Controllers
{
    [Authorize]
    [Route("cart")]
    public class CartController : ApiController
     {
        public CartController(ISender sender) : base(sender)
        {
        }

        [HttpPost]
        [Route("add-to-cart")]
        public async Task<IActionResult> AddItemToCart(AddToCartRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) {
                return Unauthorized();
            }
            var command = new AddToCartCommand(request,userId);
            var result = await Sender.Send(command);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
        [HttpGet]
        [Route("get-cart-id/")]
        public async Task<IActionResult> GetCartItemById()
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(user == null)
            {
                return Unauthorized();
            }
            var query = new GetCartByUserIdQuery(user);
            var result = await Sender.Send(query);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
        [HttpDelete]
        [Route("delete-item/{productId}/quantity/{quantity}")]
        public async Task<IActionResult> DeleteItemInCart([FromRoute]int productId, int quantity)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(userId == null)
            {
                return Unauthorized();
            }
            var command = new RemoveItemCartCommand(userId, productId);
            var result = await Sender.Send(command);
            return result.IsSuccess ? Ok(result.IsSuccess) : BadRequest(result.Error);
        }
        [HttpPost]
        [Route("checkout")]
        public async Task<IActionResult> CheckoutCart()
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (user == null)
            {
                return Unauthorized();
            }
            var command = new CheckoutCartCommand(user);
            var result = await Sender.Send(command);
            return result.IsSuccess ? Ok(result.IsSuccess) : BadRequest(result.Error);
        }
        [HttpGet]
        [Route("get-list-cart")]
        public async Task<IActionResult> GetListCartByUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(userId == null)
            {
                return Unauthorized();
            }
            var query = new GetCartByUserIdQuery(userId);
            var result = await Sender.Send(query);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
        [HttpPut]
        [Route("reduce-item/{productId}/{quantity}")]
        public async Task<IActionResult> ReduceItemInCart(int productId, int quantity)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(userId == null)
            {
                return Unauthorized();
            }
            var command = new UpdateCartCommand(productId, quantity, userId);
            var result = await Sender.Send(command);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
    }
}
