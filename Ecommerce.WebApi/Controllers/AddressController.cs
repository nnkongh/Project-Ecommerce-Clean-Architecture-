using Ecommerce.Application.Common.Command.Address.CreateAddress;
using Ecommerce.Application.Common.Command.Address.DeleteAddress;
using Ecommerce.Application.Common.Command.Address.SetDefaultAddress;
using Ecommerce.Application.Common.Command.Address.UpdateAddress;
using Ecommerce.Application.Common.Queries.Address.GetUserAddresses;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Application.DTOs.ModelsRequest.Address;
using Ecommerce.Web.ViewModels.ApiResponse;
using Ecommerce.WebApi.Controllers.BaseController;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.WebApi.Controllers
{
    [Route("addresses")]
    [Authorize]
    public class AddressController : ApiController
    {
        public AddressController(ISender sender) : base(sender)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAddresses()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var query = new GetUserAddressesQuery(userId);
            var result = await Sender.Send(query);
            return result.IsSuccess
                ? Ok(new ApiResponse<IReadOnlyList<UserAddressModel>> { IsSuccess = true, Value = result.Value })
                : BadRequest(new ApiResponse<IReadOnlyList<UserAddressModel>> { IsSuccess = false, Error = result.Error });
        }

        [HttpPost]
        public async Task<IActionResult> CreateAddress(CreateAddressRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var command = new CreateAddressCommand(request, userId);
            var result = await Sender.Send(command);
            return result.IsSuccess
                ? Ok(new ApiResponse<UserAddressModel> { IsSuccess = true, Value = result.Value })
                : BadRequest(new ApiResponse<UserAddressModel> { IsSuccess = false, Error = result.Error });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAddress(int id, UpdateAddressRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            request.Id = id;
            var command = new UpdateAddressCommand(request, userId);
            var result = await Sender.Send(command);
            return result.IsSuccess
                ? Ok(new ApiResponse<UserAddressModel> { IsSuccess = true, Value = result.Value })
                : BadRequest(new ApiResponse<UserAddressModel> { IsSuccess = false, Error = result.Error });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var command = new DeleteAddressCommand(id, userId);
            var result = await Sender.Send(command);
            return result.IsSuccess
                ? Ok(new ApiResponse<bool> { IsSuccess = true, Value = true })
                : BadRequest(new ApiResponse<bool> { IsSuccess = false, Error = result.Error });
        }

        [HttpPost("{id}/default")]
        public async Task<IActionResult> SetDefaultAddress(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var command = new SetDefaultAddressCommand(id, userId);
            var result = await Sender.Send(command);
            return result.IsSuccess
                ? Ok(new ApiResponse<bool> { IsSuccess = true, Value = true })
                : BadRequest(new ApiResponse<bool> { IsSuccess = false, Error = result.Error });
        }
    }
}
