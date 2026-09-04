using Ecommerce.Application.DTOs.ModelsRequest.Carts;
using Ecommerce.Application.Interfaces;
using Ecommerce.Web.Interface;
using Ecommerce.Web.Models;
using Ecommerce.Web.Services;
using Ecommerce.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Runtime.CompilerServices;

namespace Ecommerce.Web.Controllers
{
    
    public class CartController : Controller
    {
        private readonly CartService _cartService;
        private readonly ICheckoutCartClient _checkoutClient;
        private readonly ILogger<CartController> _logger;
        public CartController(CartService cartService, ILogger<CartController> logger, ICheckoutCartClient checkoutClient)
        {
            _cartService = cartService;
            _logger = logger;
            _checkoutClient = checkoutClient;
        }


        [HttpPost("add")]
        public async Task<IActionResult> AddToCart(AddToCartRequest request)
        {
            await _cartService.AddToCartAsync(request);
            _logger.LogInformation("Product {ProductId} added to cart.", request.Id);
            TempData["Success"] = $"Đã thêm \"{request.name}\" vào giỏ hàng";
            return RedirectToAction("Index","Category");
        }
        [HttpDelete]
        public IActionResult ClearCart()
        {
            _cartService.ClearCartAsync();
            TempData["Success"] = "Đã xóa toàn bộ giỏ hàng";
            return RedirectToAction("Index", "Category");
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var cart = await _cartService.GetCartAsync();
            return View(cart);
        }
        [HttpPost("{productId}")]
        public async Task<IActionResult> UpdateQuantity(int productId, int quantity)
        {
            await _cartService.UpdateCartAsync(productId, quantity);
            TempData["Success"] = "Đã cập nhật số lượng";
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            if(!await _cartService.RemoveFromCartAsync(productId))
            {
                ModelState.AddModelError(string.Empty, "Failed to remove item from cart.");
                return View();
            }
            TempData["Success"] = "Đã xóa sản phẩm khỏi giỏ hàng";
            return RedirectToAction("Index");
        }
       

        [HttpPost("checkout")]
        [Authorize]
        public async Task<IActionResult> CheckoutCart()
        {
            var orderViewModel = await _checkoutClient.CheckoutCartAsync();
            if (!orderViewModel.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, orderViewModel.Message);
                return View();
            }
            return RedirectToAction("Index","Order");
        }
    }
}
