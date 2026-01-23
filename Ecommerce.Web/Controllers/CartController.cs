using Ecommerce.Application.DTOs.ModelsRequest.Carts;
using Ecommerce.Application.Interfaces;
using Ecommerce.Web.Interface;
using Ecommerce.Web.Models;
using Ecommerce.Web.Services;
using Ecommerce.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            return RedirectToAction("Index","Category");
        }
        [HttpDelete]
        public IActionResult ClearCart()
        {
            _cartService.ClearCartAsync();
            return RedirectToAction("Index", "Category");
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var cart = await _cartService.GetCartAsync();
            return View(cart);
        }
        [HttpPut("${productId}")]
        public IActionResult UpdateCart(int productId, int quantity)
        {
            _cartService.UpdateCartAsync(productId, quantity);
            return RedirectToAction("Index", "Category");
        }
        [HttpDelete("{productId}")]
        public IActionResult RemoveFromCart(int productId)
        {
            _cartService.RemoveFromCartAsync(productId);
            return RedirectToAction("Index", "Category");
        }
       

        [HttpPost("checkout")]
        [Authorize]
        public async Task<IActionResult> CheckoutCart()
        {
            var orderViewModel = await _checkoutClient.CheckoutCartAsync();
            return View("Index", orderViewModel.Value);
        }
    }
}
