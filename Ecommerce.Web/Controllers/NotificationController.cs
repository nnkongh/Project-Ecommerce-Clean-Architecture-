using Ecommerce.Web.Interface;
using Ecommerce.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Web.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly INotificationClient _notificationClient;

        public NotificationController(INotificationClient notificationClient)
        {
            _notificationClient = notificationClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _notificationClient.GetMyNotificationsAsync();
            if (!result.IsSuccess)
            {
                return View(new List<NotificationViewModel>());
            }
            return View(result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> Recent()
        {
            var result = await _notificationClient.GetMyNotificationsAsync();
            var notifications = result.IsSuccess && result.Value != null
                ? result.Value.Take(5).ToList()
                : new List<NotificationViewModel>();
            return PartialView("_Recent", notifications);
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsRead(int? id)
        {
            var result = await _notificationClient.MarkAsReadAsync(id);
            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("notification/unread-count")]
        public async Task<IActionResult> UnreadCount()
        {
            var result = await _notificationClient.GetUnreadCountAsync();
            return Json(new { count = result.Value });
        }
    }
}
