using CloudinaryDotNet.Actions;
using Ecommerce.Domain.Shared;
using Ecommerce.Infrastructure.Interfaces;
using Ecommerce.Web.Interface;
using Ecommerce.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;

namespace Ecommerce.Web.Controllers
{
    public class ProductController : Controller
    {

        private readonly IPhotoService _photoService;
        private readonly IProductClient _productClient;
        public ProductController(IPhotoService photoService, IProductClient productClient)
        {
            _photoService = photoService;
            _productClient = productClient;
        }


        public IActionResult Index() => View();

        [HttpGet]
        public IActionResult Create() => View();
        [HttpPost]
        public async Task<IActionResult> Create([FromForm]ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                if(model.Image != null && model.Image.Length > 0)
                {
                    var upload = await _photoService.AddPhotoAsync(model.Image);

                    if(upload.Error != null)
                    {
                        ModelState.AddModelError("", "Lỗi upload file");
                    }
                    model.ImageUrl = upload.SecureUrl.ToString();
                }
                var result = await _productClient.CreateProductAsync(model);
                if (!result.IsSuccess)
                {
                    TempData["Failed"] = "Tạo sản phẩm thất bại";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Success"] = "Tạo sản phẩm thành công";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi tạo sản phẩm: " + ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public async  Task<IActionResult> Edit(int id)
        {
            try
            {
                var product = await _productClient.GetProductByIdAsync(id);
                if (!product.IsSuccess)
                {
                    ModelState.AddModelError("", "Lỗi upload ảnh: " + product.Error.Message);
                    return RedirectToAction(nameof(Index));
                }
                return View(product.Value);
            }
            catch
            {
                TempData["Error"] = "Có lỗi xảy ra: ";
                return RedirectToAction(nameof (Index));
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
                TempData["Success"] = "Cập nhật thành công";
                return RedirectToAction(nameof(Index));

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
                    if(!string.IsNullOrEmpty(publicId))
                    {
                        await _photoService.DeletePhotoAsync(publicId);
                    }
                }
                await _productClient.DeleteProductAsync(id);
                TempData["Success"] = "Xóa sản phẩm thành công";
            }catch (Exception)
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
    }

}
