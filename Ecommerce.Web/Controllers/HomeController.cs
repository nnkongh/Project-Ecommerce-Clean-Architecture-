using Ecommerce.Web.Interface;
using Ecommerce.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICategoryClient _categoryClient;
        private readonly IProductClient _productClient;

        public HomeController(ICategoryClient categoryClient, IProductClient productClient)
        {
            _categoryClient = categoryClient;
            _productClient = productClient;
        }

        public async Task<IActionResult> Index()
        {
            var categoriesResult = await _categoryClient.GetRootCategoriesAsync();
            var categories = categoriesResult.IsSuccess ? categoriesResult.Value?.ToList() ?? new() : new();

            var productsResult = await _productClient.GetFilteredProductsAsync(sortBy: "newest");
            var latestProducts = productsResult.IsSuccess ? productsResult.Value?.ToList() ?? new() : new();

            ViewBag.Categories = categories;
            ViewBag.LatestProducts = latestProducts.Take(8).ToList();

            return View();
        }
    }
}
