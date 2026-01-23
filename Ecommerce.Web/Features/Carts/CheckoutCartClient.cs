using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Web.Interface;
using Ecommerce.Web.ViewModels;
using Ecommerce.Web.ViewModels.ApiResponse;

namespace Ecommerce.Web.Features.Carts
{
    public class CheckoutCartClient : ICheckoutCartClient
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        public CheckoutCartClient(IHttpClientFactory clientFactory, IMapper mapper)
        {
            _httpClient = clientFactory.CreateClient("ApiClient");
            _mapper = mapper;
        }

        public async Task<ApiResponse<OrderViewModel>> CheckoutCartAsync()
        {
            var response = await _httpClient.PostAsync("carts/checkout", null);

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<OrderModel>>();

            if(result == null || !result.IsSuccess)
            {
                return ApiResponse<OrderViewModel>.Fail("Failed to checkout cart.");
            }
            
            var orderViewModel = _mapper.Map<OrderViewModel>(result.Value);

            return ApiResponse<OrderViewModel>.Success(orderViewModel);
        }
    }
}
