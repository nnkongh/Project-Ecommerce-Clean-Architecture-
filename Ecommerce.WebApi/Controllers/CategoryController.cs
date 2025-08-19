using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.WebApi.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
