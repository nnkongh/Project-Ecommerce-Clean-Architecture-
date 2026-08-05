using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Web.Interface;
using Ecommerce.Web.ViewModels;
using Ecommerce.Web.ViewModels.ApiResponse;

namespace Ecommerce.Web.Services
{
    public class WishlistClient : IWishlistClient
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public WishlistClient(IHttpClientFactory httpClient, IMapper mapper)
        {
            _mapper = mapper;
            _httpClient = httpClient.CreateClient("ApiClient");
        }

        public async Task<ApiResponse<WishlistViewModel?>> GetMyWishlistAsync()
        {
            var response = await _httpClient.GetAsync("wishlist/my");
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<WishlistModel?>>();

            if (result == null)
            {
                return ApiResponse<WishlistViewModel?>.Fail("Không thể lấy wishlist");
            }
            var mapped = _mapper.Map<WishlistViewModel>(result.Value);
            return ApiResponse<WishlistViewModel?>.Success(mapped);
        }

        public async Task<ApiResponse<WishlistViewModel>> AddItemToWishlistAsync(int productId)
        {
            var request = new { productId };
            var response = await _httpClient.PostAsJsonAsync("wishlist", request);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<WishlistModel>>();

            if (result == null || !result.IsSuccess)
            {
                return ApiResponse<WishlistViewModel>.Fail(result?.Error?.Message ?? "Không thể thêm vào wishlist");
            }
            var mapped = _mapper.Map<WishlistViewModel>(result.Value);
            return ApiResponse<WishlistViewModel>.Success(mapped);
        }

        public async Task<ApiResponse<bool>> RemoveItemFromWishlistAsync(int productId, int wishlistId)
        {
            var response = await _httpClient.DeleteAsync($"wishlist/{productId}?wishlistId={wishlistId}");

            if (!response.IsSuccessStatusCode)
            {
                return ApiResponse<bool>.Fail("Không thể xóa sản phẩm khỏi wishlist");
            }
            return ApiResponse<bool>.Success(true);
        }

        public async Task<ApiResponse<CartViewModel>> MoveItemToCartAsync(int wishlistId, int productId)
        {
            var request = new { productId };
            var response = await _httpClient.PostAsJsonAsync($"wishlist/{wishlistId}", request);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<CartModel>>();

            if (result == null || !result.IsSuccess)
            {
                return ApiResponse<CartViewModel>.Fail(result?.Error?.Message ?? "Không thể chuyển vào giỏ hàng");
            }
            var mapped = _mapper.Map<CartViewModel>(result.Value);
            return ApiResponse<CartViewModel>.Success(mapped);
        }
    }
}
