using Ecommerce.Application.Common.Command.Profile;
using Ecommerce.Application.Common.Queries.Profile.GetProfile;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Application.DTOs.ModelsRequest.User;
using Ecommerce.Web.ViewModels.ApiResponse;
using Ecommerce.WebApi.Controllers.BaseController;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.WebApi.Controllers
{
    [Authorize]
    [Route("profile")]
    public class ProfileController : ApiController
    {
        private readonly ILogger<ProfileController> logger;
        public ProfileController(ISender sender, ILogger<ProfileController> logger) : base(sender)
        {
            this.logger = logger;
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateAddress(UpdateAddressRequest request)
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (user == null) 
            { 
                return Unauthorized();
            }
            var command = new UpdateAddressCommand(user, request);
            var result = await Sender.Send(command);
            return result.IsSuccess ? Ok(new ApiResponse<AddressModel> { IsSuccess = true, Value = result.Value})
                                    : BadRequest(new ApiResponse<AddressModel> { IsSuccess = false, Error = result.Error });
        }
        [HttpGet]
        [Route("view")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            var query = new GetProfileQuery(userId);
            var result = await Sender.Send(query);
            return result.IsSuccess ? Ok(new ApiResponse<ProfileModel> { IsSuccess = true, Value = result.Value })
                                     : BadRequest(new ApiResponse<ProfileModel> { IsSuccess = false, Error = result.Error });
        }

    }
}
