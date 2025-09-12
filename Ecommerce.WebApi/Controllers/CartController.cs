using Ecommerce.Application.Common.Command.Carts.AddToCart;
using Ecommerce.Application.Common.Command.Carts.RemoveItemInCart;
using Ecommerce.Application.Common.Queries.Carts.GetCart;
using Ecommerce.Application.DTOs.CRUD.Cart;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.WebApi.Controllers.BaseController;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Threading.Tasks;

namespace Ecommerce.WebApi.Controllers
{
    [Authorize]
    [Route("Cart")]
    public class CartController : ApiController
     {
        public CartController(ISender sender) : base(sender)
        {
        }

        [HttpPost]
        [Route("add-to-cart")]
        public async Task<IActionResult> AddItemToCart(AddToCartRequest request)
        {
            var command = new AddToCartCommand(request);
            var result = await Sender.Send(command);
            return result.IsSuccess ? Ok(result.IsSuccess) : BadRequest(result.IsFailure);
        }
        [HttpGet]
        [Route("get-cart-id/{id}")]
        public async Task<ActionResult<IReadOnlyList<CartItemModel>>> GetCartItemById(int id)
        {
            var command = new GetItemQueries(id);
            var result = await Sender.Send(command);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.IsFailure);
        }
        [HttpDelete]
        [Route("delete-item/{productId}/{userId}")]
        public async Task<IActionResult> DeleteItemInCart([FromRoute]int productId, [FromRoute]string userId)
        {
            var command = new RemoveItemCartCommand(productId, userId);
            var result = await Sender.Send(command);
            return result.IsSuccess ? Ok(result.IsSuccess) : BadRequest(result.IsFailure);
        }
    }
}
