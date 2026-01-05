using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Shared;
using Ecommerce.Web.Interface;

namespace Ecommerce.Web.Services
{
    public class ProfileService : IProfileService
    {
        private readonly HttpClient _httpClient;

        public ProfileService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        public async Task<Result<ProfileModel>> GetProfileAsync()
        {
            var response = await _httpClient.GetAsync("profile/view");

            if (!response.IsSuccessStatusCode)
            {
                return Result.Failure<ProfileModel>(Error.None);
            }
            var profile = await response.Content.ReadFromJsonAsync<ProfileModel>();

            if(profile == null)
            {
                return Result.Failure<ProfileModel>(Error.NullValue);
            }
            return Result.Success(profile);
        }
    }
}
