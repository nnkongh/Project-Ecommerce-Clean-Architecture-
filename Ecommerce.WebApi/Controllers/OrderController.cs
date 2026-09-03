using Ecommerce.Application.Common.Command.Orders.CreateOrder;
using Ecommerce.Application.Common.Command.Orders.UpdateOrder;
using Ecommerce.Application.Common.Command.Shops;
using Ecommerce.Application.Common.Queries.Orders;
using Ecommerce.Application.Common.Queries.Orders.GetListOrderByUserId;
using Ecommerce.Application.Common.Queries.Orders.GetOrderById;
using Ecommerce.Application.Common.Queries.Orders.GetOrdersByShop;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Application.DTOs.ModelsRequest.Order;
using Ecommerce.Web.ViewModels.ApiResponse;
using Ecommerce.WebApi.Controllers.BaseController;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Security.Claims;

namespace Ecommerce.WebApi.Controllers
{
    [Authorize]
    [Route("order")]
    public class OrderController : ApiController
    {
        public OrderController(ISender sender) : base(sender)
        {
        }
        [HttpGet]
        public async Task<IActionResult> GetListOrderByUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(userId == null)
            {
                return Unauthorized();
            }
            var query = new GetListOrderQuery(userId);
            var result = await Sender.Send(query);
            return result.IsSuccess ? Ok(new ApiResponse<IReadOnlyList<OrderModel>> { IsSuccess = true, Value = result.Value })
                                    : BadRequest(new ApiResponse<OrderModel> { IsSuccess = false, Error = result.Error });
        }
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var query = new GetOrderByIdQuery(orderId);
            var result = await Sender.Send(query);
            return result.IsSuccess ? Ok(new ApiResponse<OrderModel> { IsSuccess = true, Value = result.Value })
                                    : BadRequest(new ApiResponse<OrderModel> { IsSuccess = false, Error = result.Error });
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderApiRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            var command = new CreateOrderCommand(request.ProductId, userId, request.Quantity);
            var result = await Sender.Send(command);
            return result.IsSuccess ? Ok(new ApiResponse<OrderModel> { IsSuccess = true, Value = result.Value })
                                    : BadRequest(new ApiResponse<OrderModel> { IsSuccess = false, Error = result.Error });
        }
        [HttpPut("{orderId}")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId) {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            var handler = new AcceptOrderCommand(userId, orderId);
            var result = await Sender.Send(handler);

            return result.IsSuccess ? Ok(new ApiResponse<OrderModel> { IsSuccess = true})
                                    : BadRequest(new ApiResponse<OrderModel> { IsSuccess = false, Error = result.Error });
        }

        [HttpGet("shop")]
        public async Task<IActionResult> GetOrdersByShop()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            var query = new GetOrdersByShopQuery(userId);
            var result = await Sender.Send(query);
            return result.IsSuccess ? Ok(new ApiResponse<IReadOnlyList<OrderModel>> { IsSuccess = true, Value = result.Value })
                                    : BadRequest(new ApiResponse<IReadOnlyList<OrderModel>> { IsSuccess = false, Error = result.Error });
        }

        [HttpPut("{orderId}/reject")]
        public async Task<IActionResult> RejectOrder(int orderId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            var command = new RejectOrderCommand(userId, orderId);
            var result = await Sender.Send(command);

            return result.IsSuccess ? Ok(new ApiResponse<bool> { IsSuccess = true })
                                    : BadRequest(new ApiResponse<bool> { IsSuccess = false, Error = result.Error });
        }
    }

    public record CreateOrderApiRequest(int ProductId, int Quantity);
}
