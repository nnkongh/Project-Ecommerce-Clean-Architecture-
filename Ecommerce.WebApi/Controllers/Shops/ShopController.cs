using Ecommerce.Application.Common.Command.Shops;
using Ecommerce.Application.Common.Queries.Shops.GetShopByUserId;
using Ecommerce.Web.ViewModels.ApiResponse;
using Ecommerce.WebApi.Controllers.BaseController;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.WebApi.Controllers.Shops
{
    [Route("shops")]
    [Authorize]
    public class ShopController : ApiController
    {
        public ShopController(ISender sender) : base(sender)
        {
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMyShop()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var query = new GetShopByUserIdQuery(userId);
            var result = await Sender.Send(query);
            return result.IsSuccess
                ? Ok(new ApiResponse<ShopModel> { IsSuccess = true, Value = result.Value })
                : Ok(new ApiResponse<ShopModel> { IsSuccess = false, Error = result.Error });
        }

        [HttpPost]
        public async Task<IActionResult> CreateShop([FromBody] CreateShopRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var command = new CreateShopCommand(userId, request.Name);
            var result = await Sender.Send(command);
            return result.IsSuccess
                ? Ok(new ApiResponse<ShopModel> { IsSuccess = true, Value = result.Value })
                : BadRequest(new ApiResponse<ShopModel> { IsSuccess = false, Error = result.Error });
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateShop(int id, [FromBody] UpdateShopRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var command = new UpdateShopCommand(userId, id, request.Name);
            var result = await Sender.Send(command);
            return result.IsSuccess
                ? Ok(new ApiResponse<ShopModel> { IsSuccess = true, Value = result.Value })
                : BadRequest(new ApiResponse<ShopModel> { IsSuccess = false, Error = result.Error });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShop(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var command = new DeleteShopCommand(userId, id);
            var result = await Sender.Send(command);
            return result.IsSuccess
                ? Ok(new ApiResponse<bool> { IsSuccess = true })
                : BadRequest(new ApiResponse<bool> { IsSuccess = false, Error = result.Error });
        }
    }

    public record CreateShopRequest(string Name);
    public record UpdateShopRequest(string Name);
}
