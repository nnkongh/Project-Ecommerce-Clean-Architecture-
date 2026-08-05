using Ecommerce.Application.Common.Command.Products.UpdateProduct;
using Ecommerce.Application.Common.Command.Profile;
using Ecommerce.Application.Common.Command.Profile.SetAvatar;
using Ecommerce.Application.Common.Queries.Profile.GetProfile;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Application.DTOs.ModelsRequest.Users;
using Ecommerce.Web.ViewModels.ApiResponse;
using Ecommerce.WebApi.Controllers.BaseController;
using Ecommerce.WebApi.Extensions;
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

        [HttpPatch("update")]
        public async Task<IActionResult> UpdateProfile(ProfileModel request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var command = new UpdateProfileCommand(userId, request);
            var result = await Sender.Send(command);
            
            return result.IsSuccess ? Ok(result) : BadRequest(result.Error);
        }
        [HttpGet("view")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            var query = new GetProfileQuery(userId);
            var result = await Sender.Send(query);
            return result.IsSuccess ? Ok(result) : BadRequest(result.Error);
        }
        [HttpPost("set-avatar")]
        public async Task<IActionResult> SetAvatar([FromForm]IFormFile file)
        {
            var user = User.GetUserId();
            if (user == null) return Unauthorized();

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var bytes = memoryStream.ToArray();

            var command = new SetAvatarCommand(bytes, file.FileName,user);
            var result = await Sender.Send(command);

            return result.IsSuccess ? Ok(result) : BadRequest(result.Error);
        }
    }
}
