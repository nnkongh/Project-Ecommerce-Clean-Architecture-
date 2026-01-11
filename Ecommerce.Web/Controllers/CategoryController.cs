using Ecommerce.Web.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Ecommerce.Web.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ICategoryClient _categoryClient;

        public CategoryController(ICategoryClient categoryClient)
        {
            _categoryClient = categoryClient;
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
        public async Task<IActionResult> ChildCategories(int id)
        {
            var result = await _categoryClient.GetChildCategoriesAsync(id);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Error.Message);
                return RedirectToAction("Login", "Auth");
            }
            ViewBag.ParentCategoryId = id;
            return View(result.Value);
        }
    }
}
