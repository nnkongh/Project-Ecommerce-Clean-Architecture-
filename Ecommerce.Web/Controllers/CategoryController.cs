using AutoMapper;
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
        private readonly IMapper _mapper;

        public CategoryController(ICategoryClient categoryClient, IProductClient productClient, IMapper mapper)
        {
            _categoryClient = categoryClient;
            _productClient = productClient;
            _mapper = mapper;
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
        public async Task<IActionResult> ChildCategories(int id, int? selectedCategoryId = null)
        {
            var detailResult = await _categoryClient.GetCategoryDetailAsync(id, selectedCategoryId);

            if (!detailResult.IsSuccess || detailResult.Value == null)
            {
                return NotFound();
            }

            var detail = detailResult.Value;
            var categories = _mapper.Map<IReadOnlyList<CategoryViewModel>>(detail.ChildCategories);

            ViewBag.ParentCategoryId = id;
            ViewBag.SelectedCategoryId = selectedCategoryId;
            ViewBag.TotalProducts = detail.DisplayProducts?.TotalItems ?? 0;

            return View(categories);
        }
    }
}
