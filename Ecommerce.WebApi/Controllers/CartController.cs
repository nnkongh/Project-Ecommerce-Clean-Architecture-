using Ecommerce.Application.Common.Command.Carts.AddToCart;
using Ecommerce.Application.Common.Command.Carts.CheckoutCart;
using Ecommerce.Application.Common.Command.Carts.RemoveItemInCart;
using Ecommerce.Application.Common.Command.Carts.UpdateCart;
using Ecommerce.Application.Common.Queries.Carts.GetCartById;
using Ecommerce.Application.Common.Queries.Carts.GetCartByUserId;
using Ecommerce.Application.DTOs.Authentication;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Application.DTOs.ModelsRequest.Carts;
using Ecommerce.Web.ViewModels.ApiResponse;
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
    [Route("carts")]
    public class CartController : ApiController
     {
        public CartController(ISender sender) : base(sender)
        {
        }

        [HttpPost]
        public async Task<IActionResult> AddItemToCart(AddToCartRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) {
                return Unauthorized();
            }
            var command = new AddToCartCommand(request,userId);
            var result = await Sender.Send(command);
            return result.IsSuccess ? Ok(new ApiResponse<CartModel> { Value = result.Value ,IsSuccess = true} ) 
                                    : BadRequest(new ApiResponse<CartModel> { IsSuccess = false, Error = result.Error});
        }
        [HttpGet]
        public async Task<IActionResult> GetCartItemById()
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(user == null)
            {
                return Unauthorized();
            }
            var query = new GetCartByUserIdQuery(user);
            var result = await Sender.Send(query);
            return result.IsSuccess ? Ok(new ApiResponse<CartModel> { Value = result.Value, IsSuccess = true}) 
                                    : BadRequest(new ApiResponse<CartModel> {IsSuccess = false, Error = result.Error});
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteItemInCart([FromRoute]int productId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(userId == null)
            {
                return Unauthorized();
            }
            var command = new RemoveItemCartCommand(userId, productId);
            var result = await Sender.Send(command);
            return result.IsSuccess ? Ok(new ApiResponse<bool> { IsSuccess = true }) 
                                    : BadRequest(new ApiResponse<bool> { IsSuccess = false, Error = result.Error});
        }
        [HttpPost("checkout")]
        public async Task<IActionResult> CheckoutCart()
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (user == null)
            {
                return Unauthorized();
            }
            var command = new CheckoutCartCommand(user);
            var result = await Sender.Send(command);
            return result.IsSuccess ? Ok(new ApiResponse<OrderModel> { Value = result.Value, IsSuccess = true}) 
                                    : BadRequest(new ApiResponse<OrderModel> { IsSuccess = false, Error = result.Error});
        }
        [HttpGet("list/item")]
        public async Task<IActionResult> GetListCartByUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(userId == null)
            {
                return Unauthorized();
            }
            var query = new GetCartByUserIdQuery(userId);
            var result = await Sender.Send(query);
            return result.IsSuccess ? Ok(new ApiResponse<CartModel> { Value = result.Value, IsSuccess = true }) 
                                    : BadRequest(new ApiResponse<CartModel> { IsSuccess = false, Error = result.Error});
        }
        [HttpPut("{productId}")]
        public async Task<IActionResult> UpdateItemQuantity(int productId,[FromQuery] int quantity)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(userId == null)
            {
                return Unauthorized();
            }
            var command = new UpdateCartCommand(productId, quantity, userId);
            var result = await Sender.Send(command);
            return result.IsSuccess ? Ok(new ApiResponse<CartModel> { Value = result.Value, IsSuccess = true }) 
                                    : BadRequest(new ApiResponse<CartModel> { IsSuccess = false, Error = result.Error });
        }
    }
}
