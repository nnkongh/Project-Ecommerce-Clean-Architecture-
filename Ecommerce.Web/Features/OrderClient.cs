using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Application.DTOs.ModelsRequest.Order;
using Ecommerce.Web.Interface;
using Ecommerce.Web.ViewModels;
using Ecommerce.Web.ViewModels.ApiResponse;

namespace Ecommerce.Web.Services
{
    public class OrderClient : IOrderClient
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public OrderClient(IHttpClientFactory httpClient, IMapper mapper)
        {
            _mapper = mapper;
            _httpClient = httpClient.CreateClient("ApiClient");
        }

        public async Task<ApiResponse<OrderViewModel>> CreatOrderAsync(CreateOrderRequest request)
        {
            var reponse = await _httpClient.PostAsJsonAsync($"order",request);

            var result = await reponse.Content.ReadFromJsonAsync<ApiResponse<OrderViewModel>>();

            if (result == null || !result.IsSuccess)
            {
                return ApiResponse<OrderViewModel>.Fail(result?.Message ?? "Failed to retrieve orders.");
            }

            var orderViewModel = _mapper.Map<OrderViewModel>(result.Value);

            return ApiResponse<OrderViewModel>.Success(orderViewModel);
        }

        public async Task<ApiResponse<IReadOnlyList<OrderViewModel>>> GetListOrderAsync()
        {
            var response = await _httpClient.GetAsync("order");

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<OrderModel>>>();

            if (result == null || !result.IsSuccess)
            {
                return ApiResponse<IReadOnlyList<OrderViewModel>>.Fail(result?.Message ?? "Failed to retrieve orders.");
            }

            var orderViewModel = _mapper.Map<IReadOnlyList<OrderViewModel>>(result.Value);

            return ApiResponse<IReadOnlyList<OrderViewModel>>.Success(orderViewModel);
        }

        public async Task<ApiResponse<OrderViewModel>> GetOrderByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"order/{id}");

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<OrderModel>>();

            if (result == null || !result.IsSuccess)
            {
                return ApiResponse<OrderViewModel>.Fail(result?.Message ?? $"Failed to retrieve order with ID {id}.");
            }

            var orderViewModel = _mapper.Map<OrderViewModel>(result.Value);

            return ApiResponse<OrderViewModel>.Success(orderViewModel);

        }

        public async Task<ApiResponse<bool>> UpdateOrderStatusAsync(int orderId)
        {
            var response = await _httpClient.PutAsync($"order/{orderId}", null);

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<OrderModel>>();

            if (result == null || !result.IsSuccess)
            {
                return ApiResponse<bool>.Fail(result?.Message ?? $"Failed to retrieve order with ID {orderId}.");
            }
            return ApiResponse<bool>.Success(true);
        }

        public async Task<ApiResponse<IReadOnlyList<OrderViewModel>>> GetOrdersByShopAsync()
        {
            var response = await _httpClient.GetAsync("order/shop");

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<OrderModel>>>();

            if (result == null || !result.IsSuccess)
            {
                return ApiResponse<IReadOnlyList<OrderViewModel>>.Fail(result?.Message ?? "Không tìm thấy đơn hàng");
            }

            var orderViewModel = _mapper.Map<IReadOnlyList<OrderViewModel>>(result.Value);
            return ApiResponse<IReadOnlyList<OrderViewModel>>.Success(orderViewModel);
        }

        public async Task<ApiResponse<bool>> RejectOrderAsync(int orderId)
        {
            var response = await _httpClient.PutAsync($"order/{orderId}/reject", null);

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();

            if (result == null || !result.IsSuccess)
            {
                return ApiResponse<bool>.Fail(result?.Message ?? $"Không thể từ chối đơn hàng #{orderId}.");
            }
            return ApiResponse<bool>.Success(true);
        }

    }
}
