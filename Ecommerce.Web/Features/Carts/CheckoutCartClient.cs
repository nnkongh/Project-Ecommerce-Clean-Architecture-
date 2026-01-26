using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Web.Interface;
using Ecommerce.Web.ViewModels;
using Ecommerce.Web.ViewModels.ApiResponse;
using System.Data.SqlTypes;

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
            try
            {
                var response = await _httpClient.PostAsync("carts/checkout", null);

                if (!response.IsSuccessStatusCode)
                {
                    return ApiResponse<OrderViewModel>.Fail("Failed to checkout cart.");
                }

                return ApiResponse<OrderViewModel>.Success(null);
            }
            catch(Exception ex)
            {
                return ApiResponse<OrderViewModel>.Fail($"Failed do: {ex}");
            }
           
        }
    }
}
