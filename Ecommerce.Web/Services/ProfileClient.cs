using AutoMapper;
using Ecommerce.Application.DTOs.Models;
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

        public ProfileClient(IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _mapper = mapper;
        }

        public async Task<ApiResponse<ProfileViewModel>> GetProfileAsync()
        {
            var response = await _httpClient.GetAsync("profile/view");

            if (!response.IsSuccessStatusCode)
            {
                return ApiResponse<ProfileViewModel>.Fail("Lỗi không thể lấy profile");
            }
            var profile = await response.Content.ReadFromJsonAsync<ApiResponse<ProfileModel>>();
            if (profile == null)
            {
                return ApiResponse<ProfileViewModel>.Fail("Lỗi không thể lấy profile");
            }
            var mapped = _mapper.Map<ProfileViewModel>(profile.Value);
            if(profile == null)
            {
                return ApiResponse<ProfileViewModel>.Fail("Lỗi không thể lấy profile");
            }
            return ApiResponse<ProfileViewModel>.Success(mapped);
        }
    }
}
