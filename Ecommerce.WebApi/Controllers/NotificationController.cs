using Ecommerce.Application.Common.Command.Notifications.MarkNotificationRead;
using Ecommerce.Application.Common.Queries.Notifications.GetUnreadNotificationCount;
using Ecommerce.Application.Common.Queries.Notifications.GetUserNotifications;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Web.ViewModels.ApiResponse;
using Ecommerce.WebApi.Controllers.BaseController;
using Ecommerce.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.WebApi.Controllers
{
    [Route("notification")]
    [Authorize]
    public class NotificationController : ApiController
    {
        public NotificationController(ISender sender) : base(sender)
        {
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMyNotifications()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            var query = new GetUserNotificationsQuery(userId);
            var result = await Sender.Send(query);
            return result.IsSuccess
                ? Ok(new ApiResponse<IReadOnlyList<NotificationModel>> { IsSuccess = true, Value = result.Value })
                : BadRequest(new ApiResponse<IReadOnlyList<NotificationModel>> { IsSuccess = false, Error = result.Error });
        }

        [HttpGet("unread-count")]
        public async Task<IActionResult> GetUnreadCount()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            var query = new GetUnreadNotificationCountQuery(userId);
            var result = await Sender.Send(query);
            return result.IsSuccess
                ? Ok(new ApiResponse<int> { IsSuccess = true, Value = result.Value })
                : BadRequest(new ApiResponse<int> { IsSuccess = false, Error = result.Error });
        }

        [HttpPost("mark-read/{id?}")]
        public async Task<IActionResult> MarkAsRead(int? id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            var command = new MarkNotificationReadCommand(userId, id);
            var result = await Sender.Send(command);
            return result.IsSuccess
                ? Ok(new ApiResponse<bool> { IsSuccess = true, Value = true })
                : BadRequest(new ApiResponse<bool> { IsSuccess = false, Error = result.Error });
        }
    }
}
