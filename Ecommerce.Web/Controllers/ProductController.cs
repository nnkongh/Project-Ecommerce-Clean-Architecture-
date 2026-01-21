using CloudinaryDotNet.Actions;
using Ecommerce.Domain.Models;
using Ecommerce.Domain.Shared;
using Ecommerce.Infrastructure.Interfaces;
using Ecommerce.Web.Interface;
using Ecommerce.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace Ecommerce.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly ICategoryClient _categoryClient;
        private readonly IPhotoService _photoService;
        private readonly IProductClient _productClient;
        public ProductController(IPhotoService photoService, IProductClient productClient, ICategoryClient categoryClient)
        {
            _photoService = photoService;
            _productClient = productClient;
            _categoryClient = categoryClient;
        }


        public async Task<IActionResult> Index()
        {
            var product = await _productClient.GetAllProductsAsync();
            return View(product.Value);
        }

        [HttpGet]
        
        public async Task<IActionResult> Create()
        {
            var model = new ProductViewModel();
            await LoadCategories(model);
            return View(model);
        }
        private async Task LoadCategories(ProductViewModel model)
        {
            var categories = await _categoryClient.GetRootCategoriesAsync();

            model.ParentCategories = categories.Value.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            var allCategories = await _categoryClient.GetAllCategoriesAsync();

            if (allCategories.IsSuccess)
            {
                model.AllCategories = allCategories.Value.ToList();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                if (model.Image != null && model.Image.Length > 0)
                {
                    var upload = await _photoService.AddPhotoAsync(model.Image);

                    if (upload.Error != null)
                    {
                        ModelState.AddModelError("", "Lỗi upload file");
                    }
                    model.ImageUrl = upload.SecureUrl.ToString();
                }

                var result = await _productClient.CreateProductAsync(model);
                if (!result.IsSuccess)
                {
                    TempData["Failed"] = "Tạo sản phẩm thất bại";
                    return RedirectToAction(nameof(Create));
                }
                TempData["Success"] = "Tạo sản phẩm thành công";

                var categoryResult = await _categoryClient.GetCategoryByIdAsync(model.CategoryId);
                var category = categoryResult.Value;

                return RedirectToAction("ChildCategories", "Category", new { id = category.ParentId, selectCategoryId = category.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi tạo sản phẩm: " + ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var product = await _productClient.GetProductByIdAsync(id);

                if (!product.IsSuccess)
                {
                    ModelState.AddModelError("", "Lỗi lấy sản phẩm " + product.Error.Message);
                    return RedirectToAction(nameof(Index));
                }
                return View(product.Value);
            }
            catch
            {
                TempData["Error"] = "Có lỗi xảy ra: ";
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                if (model.Image != null && model.Image.Length > 0)
                {
                    if (!string.IsNullOrEmpty(model.ImageUrl))
                    {
                        var oldPublicId = GetPublicIdFromUrl(model.ImageUrl);
                        if (!string.IsNullOrEmpty(oldPublicId))
                        {
                            await _photoService.DeletePhotoAsync(oldPublicId);
                        }
                    }
                    var uploadResult = await _photoService.AddPhotoAsync(model.Image);
                    if (uploadResult.Error != null)
                    {
                        ModelState.AddModelError("", "Lỗi upload ảnh: " + uploadResult.Error.Message);
                        return View(model);
                    }
                    model.ImageUrl = uploadResult.SecureUrl.ToString();
                }
                var result = await _productClient.UpdateProductAsync(id, model);
                if (!result.IsSuccess)
                {
                    ModelState.AddModelError("", "Lỗi upload ảnh: " + result.Error.Message);
                    return View(model);
                }
                var categoriesResult = await _categoryClient.GetCategoryByIdAsync(model.CategoryId);
                var category = categoriesResult.Value;

                TempData["Success"] = "Cập nhật thành công";
                return RedirectToAction("ChildCategories", "Category", new { id = category.ParentId, selectCategoryId = category.Id });

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật sản phẩm: " + ex.Message);
                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var product = await _productClient.GetProductByIdAsync(id);

                if (!product.IsSuccess)
                {
                    ModelState.AddModelError("", "Lỗi upload ảnh: " + product.Error.Message);
                    return RedirectToAction(nameof(Index));
                }
                if (!string.IsNullOrEmpty(product.Value!.ImageUrl))
                {
                    var publicId = GetPublicIdFromUrl(product.Value!.ImageUrl);
                    if (!string.IsNullOrEmpty(publicId))
                    {
                        await _photoService.DeletePhotoAsync(publicId);
                    }
                }
                await _productClient.DeleteProductAsync(id);
                TempData["Success"] = "Xóa sản phẩm thành công";
            }
            catch (Exception)
            {
                TempData["Error"] = "Có lỗi xảy ra khi xóa sản phẩm!";
            }
            return RedirectToAction(nameof(Index));
        }

        private string GetPublicIdFromUrl(string imageUrl)
        {
            var uri = new Uri(imageUrl);
            var segment = uri.Segments;
            var fileName = segment[segment.Length - 1];
            return fileName;
        }

        [HttpGet("products/{categoryId}")]
        [AllowAnonymous]
        public async Task<IActionResult> Products(int categoryId)
        {
            var productsResult = await _productClient.GetAllProductsByCategoryAsync(categoryId);

            if (!productsResult.IsSuccess)
            {
                TempData["Error"] = productsResult.Error.Message;
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = categoryId;
            ViewBag.CategoryName = "Sản phẩm";

            return View(productsResult.Value);
        }

        [HttpGet("item/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Detail(int id)
        {
            var result = await _productClient.GetProductByIdAsync(id);
            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Error.Message;
                return RedirectToAction("Index");
            }
            return View(result.Value);
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Search([FromQuery]string name)
        {
            var result = await _productClient.GetAllProductsByNameAsync(name);
            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Error.Message;
                return RedirectToAction("Index");
            }
            ViewBag.SearchTerm = name;
            return View("Index",result.Value);
        }
    }

}
