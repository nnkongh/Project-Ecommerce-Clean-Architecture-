using AutoMapper;
using Ecommerce.Application.DTOs.Models;
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

        public Task<ApiResponse<OrderViewModel>> CreatOrderAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<IReadOnlyList<OrderViewModel>>> GetListOrderAsync()
        {
            var response = await _httpClient.GetAsync("orders");

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<OrderModel>>>();

            if(result == null || !result.IsSuccess)
            {
                return ApiResponse<IReadOnlyList<OrderViewModel>>.Fail(result?.Message ?? "Failed to retrieve orders.");
            }

            var orderViewModel = _mapper.Map<IReadOnlyList<OrderViewModel>>(result.Value);

            return ApiResponse<IReadOnlyList<OrderViewModel>>.Success(orderViewModel);
        }

        public async Task<ApiResponse<OrderViewModel>> GetOrderByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"orders/{id}");

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<OrderModel>>();

            if(result == null || !result.IsSuccess)
            {
                return ApiResponse<OrderViewModel>.Fail(result?.Message ?? $"Failed to retrieve order with ID {id}.");
            }

            var orderViewModel = _mapper.Map<OrderViewModel>(result.Value);

            return ApiResponse<OrderViewModel>.Success(orderViewModel);

        }
    }
}
