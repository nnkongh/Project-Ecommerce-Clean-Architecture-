using AutoMapper;
using Ecommerce.Application.Common.Command.Shops;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Web.Interface;
using Ecommerce.Web.ViewModels;
using Ecommerce.Web.ViewModels.ApiResponse;

namespace Ecommerce.Web.Features
{
    public class ShopClient : IShopClient
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public ShopClient(IHttpClientFactory httpClient, IMapper mapper)
        {
            _mapper = mapper;
            _httpClient = httpClient.CreateClient("ApiClient");
        }

        public async Task<ApiResponse<ShopViewModel>> GetMyShopAsync()
        {
            var response = await _httpClient.GetAsync("shops/my");
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<ShopModel>>();

            if (result == null || !result.IsSuccess)
            {
                return ApiResponse<ShopViewModel>.Fail(result?.Error?.Message ?? "Không tìm thấy cửa hàng");
            }

            var mapped = _mapper.Map<ShopViewModel>(result.Value);
            return ApiResponse<ShopViewModel>.Success(mapped);
        }

        public async Task<ApiResponse<ShopViewModel>> CreateShopAsync(string name)
        {
            var response = await _httpClient.PostAsJsonAsync("shops", new { Name = name });
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<ShopModel>>();

            if (result == null || !result.IsSuccess)
            {
                return ApiResponse<ShopViewModel>.Fail(result?.Error?.Message ?? "Không thể tạo cửa hàng");
            }

            var mapped = _mapper.Map<ShopViewModel>(result.Value);
            return ApiResponse<ShopViewModel>.Success(mapped);
        }

        public async Task<ApiResponse<ShopViewModel>> UpdateShopAsync(int id, string name)
        {
            var response = await _httpClient.PatchAsJsonAsync($"shops/{id}", new { Name = name });
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<ShopModel>>();

            if (result == null || !result.IsSuccess)
            {
                return ApiResponse<ShopViewModel>.Fail(result?.Error?.Message ?? "Không thể cập nhật cửa hàng");
            }

            var mapped = _mapper.Map<ShopViewModel>(result.Value);
            return ApiResponse<ShopViewModel>.Success(mapped);
        }

        public async Task<ApiResponse<bool>> DeleteShopAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"shops/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return ApiResponse<bool>.Fail("Không thể xóa cửa hàng");
            }
            return ApiResponse<bool>.Success(true);
        }
    }
}
