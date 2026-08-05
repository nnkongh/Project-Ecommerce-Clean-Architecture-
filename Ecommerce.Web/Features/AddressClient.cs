using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Application.DTOs.ModelsRequest.Address;
using Ecommerce.Web.Interface;
using Ecommerce.Web.Models;
using Ecommerce.Web.ViewModels.ApiResponse;

namespace Ecommerce.Web.Services
{
    public class AddressClient : IAddressClient
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public AddressClient(IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _mapper = mapper;
        }

        public async Task<ApiResponse<IReadOnlyList<UserAddressViewModel>>> GetAddressesAsync()
        {
            var response = await _httpClient.GetAsync("addresses");
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<UserAddressModel>>>();

            if (result == null || !result.IsSuccess)
            {
                return ApiResponse<IReadOnlyList<UserAddressViewModel>>.Fail(result?.Error?.Message ?? "Không thể lấy danh sách địa chỉ");
            }

            var mapped = _mapper.Map<IReadOnlyList<UserAddressViewModel>>(result.Value);
            return ApiResponse<IReadOnlyList<UserAddressViewModel>>.Success(mapped);
        }

        public async Task<ApiResponse<UserAddressViewModel>> CreateAddressAsync(CreateAddressRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("addresses", request);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<UserAddressModel>>();

            if (result == null || !result.IsSuccess)
            {
                return ApiResponse<UserAddressViewModel>.Fail(result?.Error?.Message ?? "Không thể tạo địa chỉ");
            }

            var mapped = _mapper.Map<UserAddressViewModel>(result.Value);
            return ApiResponse<UserAddressViewModel>.Success(mapped);
        }

        public async Task<ApiResponse<UserAddressViewModel>> UpdateAddressAsync(UpdateAddressRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"addresses/{request.Id}", request);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<UserAddressModel>>();

            if (result == null || !result.IsSuccess)
            {
                return ApiResponse<UserAddressViewModel>.Fail(result?.Error?.Message ?? "Không thể cập nhật địa chỉ");
            }

            var mapped = _mapper.Map<UserAddressViewModel>(result.Value);
            return ApiResponse<UserAddressViewModel>.Success(mapped);
        }

        public async Task<ApiResponse<bool>> DeleteAddressAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"addresses/{id}");
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();

            if (result == null || !result.IsSuccess)
            {
                return ApiResponse<bool>.Fail(result?.Error?.Message ?? "Không thể xóa địa chỉ");
            }

            return ApiResponse<bool>.Success(true);
        }

        public async Task<ApiResponse<bool>> SetDefaultAddressAsync(int id)
        {
            var response = await _httpClient.PostAsync($"addresses/{id}/default", null);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();

            if (result == null || !result.IsSuccess)
            {
                return ApiResponse<bool>.Fail(result?.Error?.Message ?? "Không thể đặt làm mặc định");
            }

            return ApiResponse<bool>.Success(true);
        }
    }
}
