using Ecommerce.Web.Interface;
using Ecommerce.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Web.Controllers
{
    [Authorize]
    public class WishlistController : Controller
    {
        private readonly IWishlistClient _wishlistClient;

        public WishlistController(IWishlistClient wishlistClient)
        {
            _wishlistClient = wishlistClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _wishlistClient.GetMyWishlistAsync();
            if (!result.IsSuccess || result.Value == null)
            {
                return View(new WishlistViewModel());
            }
            return View(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> AddToWishlist(int productId)
        {
            var result = await _wishlistClient.AddItemToWishlistAsync(productId);
            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Message;
            }
            else
            {
                TempData["Success"] = "Đã thêm vào danh sách yêu thích";
            }
            return Redirect(Request.Headers["Referer"].ToString() ?? "/");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromWishlist(int productId, int wishlistId)
        {
            var result = await _wishlistClient.RemoveItemFromWishlistAsync(productId, wishlistId);
            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("wishlist/move-to-cart/{wishlistId}")]
        public async Task<IActionResult> MoveToCart(int wishlistId, int productId)
        {
            var result = await _wishlistClient.MoveItemToCartAsync(wishlistId, productId);
            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Message;
            }
            else
            {
                TempData["Success"] = "Đã chuyển sản phẩm vào giỏ hàng";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
