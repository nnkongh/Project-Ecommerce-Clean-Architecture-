using Ecommerce.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace Ecommerce.Web.Controllers
{
    [Route("internal/notification")]
    public class InternalNotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly IConfiguration _configuration;

        public InternalNotificationController(INotificationService notificationService, IConfiguration configuration)
        {
            _notificationService = notificationService;
            _configuration = configuration;
        }

        [HttpPost("push")]
        public async Task<IActionResult> Push([FromQuery] string userId, [FromQuery] string? title, [FromQuery] string? description)
        {
            var expectedKey = _configuration["Internal:PushKey"];
            var actualKey = Request.Headers["X-Internal-Key"].FirstOrDefault();

            if (string.IsNullOrEmpty(expectedKey) ||
                string.IsNullOrEmpty(actualKey) ||
                !string.Equals(expectedKey, actualKey, System.StringComparison.Ordinal))
            {
                return Unauthorized();
            }

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            //await _notificationService.SendNotificationAsync(
            //    userId,
            //    new { title = title ?? string.Empty, description = description ?? string.Empty });

            return Ok();
        }
    }
}