using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Application.DTOs.ModelsRequest.User;
using Ecommerce.Domain.Shared;
using Ecommerce.Web.Interface;
using Ecommerce.Web.ViewModels.ApiResponse;
using Ecommerce.Web.ViewModels.Profile;

namespace Ecommerce.Web.Services
{
    public class ProfileClient : IProfileClient
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        private readonly ILogger<ProfileClient> _logger;
        public ProfileClient(IHttpClientFactory httpClientFactory, IMapper mapper, ILogger<ProfileClient> logger)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse<ProfileViewModel>> GetProfileAsync()
        {
            var response = await _httpClient.GetAsync("profile/view");

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<ProfileModel>>();

            if(result == null || !result.IsSuccess)
            {
                return ApiResponse<ProfileViewModel>.Fail(result?.Error?.Message ?? "Không thể lấy profile");
            }

            var mapped = _mapper.Map<ProfileViewModel>(result.Value);

            return ApiResponse<ProfileViewModel>.Success(mapped);
        }

        public async Task<ApiResponse<UpdateProfileRequest>> GetProfileForEditAsync()
        {
            var response = await _httpClient.GetAsync("profile/view");
            
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<ProfileModel>>();

            if (result == null || !result.IsSuccess)
            {
                return ApiResponse<UpdateProfileRequest>.Fail(result?.Error?.Message ?? "Lỗi không thể lấy profile");
            }

            var mapped = _mapper.Map<UpdateProfileRequest>(result.Value);

            return ApiResponse<UpdateProfileRequest>.Success(mapped);
        }

        public async Task<ApiResponse<ProfileViewModel>> UpdateProfileAsync(UpdateProfileRequest request)
        {
            var response = await _httpClient.PatchAsJsonAsync("profile/update", request);

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<ProfileModel>>();

            if (result == null || !result.IsSuccess)
            {
                return ApiResponse<ProfileViewModel>.Fail(result?.Error?.Message ?? "Cập nhật không thành công");
            }

            var mapped = _mapper.Map<ProfileViewModel>(result.Value);

            return ApiResponse<ProfileViewModel>.Success(mapped);
        }
    }
}
