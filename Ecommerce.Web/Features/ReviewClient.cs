using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Web.Interface;
using Ecommerce.Web.ViewModels;
using Ecommerce.Web.ViewModels.ApiResponse;

namespace Ecommerce.Web.Services
{
    public class ReviewClient : IReviewClient
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public ReviewClient(IHttpClientFactory httpClient, IMapper mapper)
        {
            _mapper = mapper;
            _httpClient = httpClient.CreateClient("ApiClient");
        }

        public async Task<ApiResponse<IReadOnlyList<ReviewViewModel>>> GetReviewsByProductIdAsync(int productId)
        {
            var response = await _httpClient.GetAsync($"reviews/product/{productId}");
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<ReviewModel>>>();

            if (result == null || !result.IsSuccess)
            {
                return ApiResponse<IReadOnlyList<ReviewViewModel>>.Fail(result?.Error?.Message ?? "Không thể lấy đánh giá");
            }

            var mapped = _mapper.Map<IReadOnlyList<ReviewViewModel>>(result.Value);
            return ApiResponse<IReadOnlyList<ReviewViewModel>>.Success(mapped);
        }

        public async Task<ApiResponse<ReviewViewModel>> CreateReviewAsync(int productId, string content, int rating)
        {
            var request = new { content, productId, rating };
            var response = await _httpClient.PostAsJsonAsync("reviews", request);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<ReviewModel>>();

            if (result == null || !result.IsSuccess)
            {
                return ApiResponse<ReviewViewModel>.Fail(result?.Error?.Message ?? "Không thể thêm đánh giá");
            }

            var mapped = _mapper.Map<ReviewViewModel>(result.Value);
            return ApiResponse<ReviewViewModel>.Success(mapped);
        }

        public async Task<ApiResponse<bool>> DeleteReviewAsync(int reviewId)
        {
            var response = await _httpClient.DeleteAsync($"reviews/{reviewId}");

            if (!response.IsSuccessStatusCode)
            {
                return ApiResponse<bool>.Fail("Không thể xóa đánh giá");
            }
            return ApiResponse<bool>.Success(true);
        }
    }
}
