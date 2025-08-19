using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.WebApi.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
