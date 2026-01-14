using Ecommerce.Domain.Models;
using Ecommerce.Web.Interface;
using Ecommerce.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Ecommerce.Web.Controllers
{
    [Authorize]
    
    public class CategoryController : Controller
    {
        private readonly ICategoryClient _categoryClient;
        private readonly IProductClient _productClient;
        public CategoryController(ICategoryClient categoryClient, IProductClient productClient)
        {
            _categoryClient = categoryClient;
            _productClient = productClient;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var result = await _categoryClient.GetRootCategoriesAsync();
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Error.Message);
                return RedirectToAction("Login", "Auth");
            }
            return View(result.Value);
        }
        [HttpGet("detailed/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> ChildCategories(int id, int? selectCategoryId = null)
        {
            var categoriesResult = await _categoryClient.GetChildCategoriesAsync(id);

            if (!categoriesResult.IsSuccess)
            {
                return NotFound();
            }

            var categories = categoriesResult.Value.ToList();
            List<ProductViewModel> displayProducts = new();

            // Nếu có chọn category con cụ thể
            if (selectCategoryId.HasValue)
            {
                var categoryProducts = await _productClient.GetAllProductsByCategoryAsync(selectCategoryId.Value);
                if (categoryProducts.IsSuccess)
                {
                    displayProducts = categoryProducts.Value.ToList();
                }
            }
            else
            {
                // Lấy products của TẤT CẢ các category con song song
                var productTasks = categories
                    .Where(c => c.Id.HasValue)
                    .Select(c => _productClient.GetAllProductsByCategoryAsync(c.Id.Value));

                var productResults = await Task.WhenAll(productTasks);

                displayProducts = productResults
                    .Where(r => r.IsSuccess)
                    .SelectMany(r => r.Value)
                    .ToList();
            }

            // GÁN products cho từng category để hiển thị
            foreach (var category in categories.Where(c => c.Id.HasValue))
            {
                var categoryProducts = await _productClient.GetAllProductsByCategoryAsync(category.Id.Value);
                if (categoryProducts.IsSuccess)
                {
                    category.Products = categoryProducts.Value.ToList();
                }
            }

            ViewBag.ParentCategoryId = id;
            ViewBag.SelectedCategoryId = selectCategoryId;
            ViewBag.DisplayProducts = displayProducts;

            return View(categories);

        }
    }
}
