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
        private readonly IOrderClient _orderClient;
        private readonly IShopClient _shopClient;

        public NotificationController(
            INotificationClient notificationClient,
            IOrderClient orderClient,
            IShopClient shopClient)
        {
            _notificationClient = notificationClient;
            _orderClient = orderClient;
            _shopClient = shopClient;
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
        public async Task<IActionResult> Details(int id)
        {
            var result = await _notificationClient.GetMyNotificationsAsync();
            var notifications = result.IsSuccess ? result.Value : new List<NotificationViewModel>();
            var notification = notifications?.FirstOrDefault(n => n.Id == id);

            if (notification == null)
            {
                return RedirectToAction(nameof(Index));
            }

            await _notificationClient.MarkAsReadAsync(id);

            var viewModel = new NotificationDetailViewModel
            {
                Notification = notification
            };

            if (notification.OrderId.HasValue)
            {
                var orderResult = await _orderClient.GetOrderByIdAsync(notification.OrderId.Value);
                if (orderResult.IsSuccess)
                {
                    var shopResult = await _shopClient.GetMyShopAsync();
                    if (shopResult.IsSuccess && shopResult.Value != null)
                    {
                        var shopId = shopResult.Value.Id;
                        viewModel.ShopSubOrder = orderResult.Value.SubOrders?
                            .FirstOrDefault(s => s.ShopId == shopId);
                    }

                    viewModel.Order = orderResult.Value;
                    viewModel.CanAccept = viewModel.ShopSubOrder != null
                        && orderResult.Value.OrderStatus == Ecommerce.Domain.Enum.OrderStatus.Pending;
                    viewModel.CanReject = viewModel.CanAccept;
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptOrder(int notificationId, int orderId)
        {
            var result = await _orderClient.UpdateOrderStatusAsync(orderId);
            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Error?.Message ?? "Không thể xác nhận đơn hàng";
            }
            else
            {
                TempData["Success"] = "Đã xác nhận đơn hàng thành công!";
            }
            return RedirectToAction(nameof(Details), new { id = notificationId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectOrder(int notificationId, int orderId)
        {
            var result = await _orderClient.RejectOrderAsync(orderId);
            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Error?.Message ?? "Không thể từ chối đơn hàng";
            }
            else
            {
                TempData["Success"] = "Đã từ chối đơn hàng.";
            }
            return RedirectToAction(nameof(Details), new { id = notificationId });
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
