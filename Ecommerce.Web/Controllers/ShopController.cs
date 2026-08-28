using Ecommerce.Application.Interfaces;
using Ecommerce.Infrastructure.Interfaces;
using Ecommerce.Web.Interface;
using Ecommerce.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Web.Controllers
{
    [Authorize]
    public class ShopController : Controller
    {
        private readonly IShopClient _shopClient;
        private readonly IProductClient _productClient;
        private readonly ICategoryClient _categoryClient;
        private readonly IPhotoService _photoService;

        public ShopController(IShopClient shopClient, IProductClient productClient, ICategoryClient categoryClient, IPhotoService photoService)
        {
            _shopClient = shopClient;
            _productClient = productClient;
            _categoryClient = categoryClient;
            _photoService = photoService;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new ShopRegisterViewModel();

            var shopResult = await _shopClient.GetMyShopAsync();
            if (shopResult.IsSuccess)
            {
                model.Shop = shopResult.Value;
                model.Shop.HasShop = true;
                await LoadCategories(model);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string shopName)
        {
            if (string.IsNullOrWhiteSpace(shopName))
            {
                TempData["Failed"] = "Tên cửa hàng không được để trống";
                return RedirectToAction(nameof(Create));
            }

            var result = await _shopClient.CreateShopAsync(shopName);
            if (!result.IsSuccess)
            {
                TempData["Failed"] = result.Error?.Message ?? "Không thể tạo cửa hàng";
                return RedirectToAction(nameof(Create));
            }

            TempData["Success"] = "Tạo cửa hàng thành công! Bây giờ bạn có thể thêm sản phẩm.";
            return RedirectToAction(nameof(Create));
        }

        [HttpGet]
        public async Task<IActionResult> Products()
        {
            var shopResult = await _shopClient.GetMyShopAsync();
            if (!shopResult.IsSuccess)
            {
                TempData["Failed"] = "Bạn chưa có cửa hàng";
                return RedirectToAction(nameof(Create));
            }

            var model = new ShopRegisterViewModel
            {
                Shop = shopResult.Value,
            };
            model.Shop.HasShop = true;

            await LoadProducts(model);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromForm] ShopRegisterViewModel model)
        {
            if (model.NewProduct == null)
            {
                TempData["Failed"] = "Dữ liệu sản phẩm không hợp lệ";
                return RedirectToAction(nameof(Create));
            }

            if (string.IsNullOrEmpty(model.NewProduct.Name))
            {
                TempData["Failed"] = "Tên sản phẩm không được để trống";
                return RedirectToAction(nameof(Create));
            }

            try
            {
                if (model.NewProduct.Image != null && model.NewProduct.Image.Length > 0)
                {
                    using var memoryStream = new MemoryStream();
                    await model.NewProduct.Image.CopyToAsync(memoryStream);
                    var bytes = memoryStream.ToArray();
                    model.NewProduct.ImageUrl = await _photoService.CreatePhotoAsync(bytes, model.NewProduct.Image.FileName);
                }

                var result = await _productClient.CreateProductAsync(model.NewProduct);
                if (!result.IsSuccess)
                {
                    TempData["Failed"] = "Tạo sản phẩm thất bại";
                    return RedirectToAction(nameof(Create));
                }

                TempData["Success"] = "Thêm sản phẩm thành công!";
                return RedirectToAction(nameof(Create));
            }
            catch (Exception ex)
            {
                TempData["Failed"] = "Có lỗi xảy ra: " + ex.Message;
                return RedirectToAction(nameof(Create));
            }
        }

        private async Task LoadCategories(ShopRegisterViewModel model)
        {
            var categories = await _categoryClient.GetRootCategoriesAsync();
            if (categories.Value != null)
            {
                model.ParentCategories = categories.Value.Select(c => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();
            }
            var allCategories = await _categoryClient.GetAllCategoriesAsync();
            if (allCategories.IsSuccess)
            {
                model.AllCategories = allCategories.Value.ToList();
            }
        }

        private async Task LoadProducts(ShopRegisterViewModel model)
        {
            if (model.Shop == null)
            {
                return;
            }
            var productsResult = await _productClient.GetAllProductsByShopIdAsync(model.Shop.Id);
            if (productsResult.IsSuccess)
            {
                model.Products = productsResult.Value.ToList();
            }
        }
    }
}
