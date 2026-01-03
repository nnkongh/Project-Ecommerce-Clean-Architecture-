using Ecommerce.Application.Common.Command.Profile;
using Ecommerce.Application.Common.Queries.Profile.GetProfile;
using Ecommerce.Application.DTOs.ModelsRequest.User;
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
        public ProfileController(ISender sender) : base(sender)
        {
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
            return result.IsSuccess ? Ok(result.Value) : BadRequest();
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
            return result.IsSuccess ? Ok(result.Value) : BadRequest();
        }

    }
}
