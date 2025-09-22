﻿using Ecommerce.Application.Common.Queries.Orders;
using Ecommerce.Application.Common.Queries.Orders.GetListOrderByUserId;
using Ecommerce.Application.Common.Queries.Orders.GetOrderById;
using Ecommerce.WebApi.Controllers.BaseController;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet("get-list-order")]
        public async Task<IActionResult> GetListOrderByUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(userId == null)
            {
                return Unauthorized();
            }
            var query = new GetListOrderQuery(userId);
            var result = await Sender.Send(query);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
        [HttpGet("get-order/{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var query = new GetOrderByIdQuery(orderId);
            var result = await Sender.Send(query);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
        
    }
}
