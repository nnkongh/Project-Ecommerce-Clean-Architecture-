using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Application.DTOs.ModelsRequest.Carts;
using Ecommerce.Domain.Shared;
using Ecommerce.Web.Services.Strategy;
using Ecommerce.Web.ViewModels;
using Ecommerce.Web.ViewModels.ApiResponse;
using System.Security.Claims;
using System.Text;

namespace Ecommerce.Web.Features.Carts
{
    public class CartApiClient : ICartStrategy
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        private readonly ILogger<CartApiClient> _l;

        public CartApiClient(IHttpClientFactory httpClientFactory, IMapper mapper, ILogger<CartApiClient> l)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _mapper = mapper;
            _l = l;
        }

        public async Task AddToCartAsync(AddToCartRequest request)
        {

            try
            {
                var response = await _httpClient.PostAsJsonAsync($"carts", request);

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<CartViewModel>>();

                if (result == null || !result.IsSuccess)
                {
                }
            }
            catch (Exception ex)
            {
                _l.LogInformation($"Message: {ex}");
                throw new Exception("Failed to add product to cart.");

            }

        }

        

        public async Task ClearCartAsync()
        {
            var response = await _httpClient.DeleteAsync("carts");

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();

            if (result == null || !result.IsSuccess)
            {
                throw new Exception("Failed to clear cart.");
            }
        }

        public async Task<CartViewModel> GetCartAsync()
        {
            var response = await _httpClient.GetAsync("carts");

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<CartModel>>();

            if (result == null || !result.IsSuccess)
            {
                return new CartViewModel { Items = new List<CartItemViewModel>() };
            }

            var mapped = _mapper.Map<CartViewModel>(result.Value);

            return mapped;
        }

        public async Task<bool> RemoveFromCartAsync(int productId)
        {
            var response = await _httpClient.DeleteAsync($"carts/{productId}");

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();

            if (result == null || !result.IsSuccess)
            {
                throw new Exception("Failed to remove product from cart.");
            }
           
            return true;
        }

        public async Task UpdateCartAsync(int productId, int quantity)
        {
            var response = await _httpClient.PutAsync($"carts/{productId}?quantity={quantity}", null);

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<CartViewModel>>();

            if (result == null || !result.IsSuccess)
            {
                throw new Exception("Failed to update cart.");
            }
        }
    }
}
