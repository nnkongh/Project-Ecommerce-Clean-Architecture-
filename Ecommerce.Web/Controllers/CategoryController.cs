using Ecommerce.Domain.Models;
using Ecommerce.Domain.Shared;
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
        public async Task<IActionResult> Index(int page = 1, int pageSize = 12, int productPage = 1)
        {
            var result = await _categoryClient.GetRootCategoriesPagedAsync(page, pageSize);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Error.Message);
                return RedirectToAction("Login", "Auth");
            }

            var products = await _productClient.GetAllProductsByPaginationAsync(productPage, 8);

            ViewBag.DisplayProducts = products;
            return View(result.Value);
        }
        [HttpGet("detailed/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> ChildCategories(int id, int? selectCategoryId = null, int page = 1, int pageSize = 12)
        {
            var categoriesResult = await _categoryClient.GetChildCategoriesPagedAsync(id, page, pageSize);

            if (!categoriesResult.IsSuccess)
            {
                return NotFound();
            }

            var categories = categoriesResult.Value.Items.ToList();

            ViewBag.ParentCategoryId = id;
            ViewBag.SelectedCategoryId = selectCategoryId;
            ViewBag.CategoryPagedResult = categoriesResult.Value;

            return View(categories);
        }
    }
}
