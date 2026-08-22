using Ecommerce.Application.DTOs.ModelsRequest.Order;
using Ecommerce.Web.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Ecommerce.Web.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderClient _orderClient;

        public OrderController(IOrderClient orderClient)
        {
            _orderClient = orderClient;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _orderClient.GetListOrderAsync();
            return View(orders.Value);
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderClient.GetOrderByIdAsync(id);
            if (!order.IsSuccess)
            {
                return NotFound();
            }
            return View(order.Value);
        }
        public async Task<IActionResult> UpdateOrderStatus(int orderId)
        {
            var order = await _orderClient.UpdateOrderStatusAsync(orderId);
            if (!order.IsSuccess)
            {
                return NoContent();
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Create(CreateOrderRequest request)
        {
            var order = await _orderClient.CreatOrderAsync(request);
            if(!order.IsSuccess)
            {
                return Content(order.Error.ToString());
            }
            return View(order);
        }
    }
}
