using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Web.Interface;
using Ecommerce.Web.ViewModels;
using Ecommerce.Web.ViewModels.ApiResponse;

namespace Ecommerce.Web.Services
{
    public class CommentClient : ICommentClient
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public CommentClient(IHttpClientFactory httpClient, IMapper mapper)
        {
            _mapper = mapper;
            _httpClient = httpClient.CreateClient("ApiClient");
        }

        public async Task<ApiResponse<IReadOnlyList<CommentViewModel>>> GetCommentsByProductIdAsync(int productId)
        {
            var response = await _httpClient.GetAsync($"comments/product/{productId}");
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<CommentModel>>>();

            if (result == null || !result.IsSuccess)
            {
                return ApiResponse<IReadOnlyList<CommentViewModel>>.Fail(result?.Error?.Message ?? "Không thể lấy bình luận");
            }

            var mapped = _mapper.Map<IReadOnlyList<CommentViewModel>>(result.Value);
            return ApiResponse<IReadOnlyList<CommentViewModel>>.Success(mapped);
        }

        public async Task<ApiResponse<CommentViewModel>> CreateCommentAsync(int productId, string content)
        {
            var request = new { content, productId };
            var response = await _httpClient.PostAsJsonAsync("comments", request);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<CommentModel>>();

            if (result == null || !result.IsSuccess)
            {
                return ApiResponse<CommentViewModel>.Fail(result?.Error?.Message ?? "Không thể thêm bình luận");
            }

            var mapped = _mapper.Map<CommentViewModel>(result.Value);
            return ApiResponse<CommentViewModel>.Success(mapped);
        }

        public async Task<ApiResponse<bool>> DeleteCommentAsync(int commentId)
        {
            var response = await _httpClient.DeleteAsync($"comments/{commentId}");

            if (!response.IsSuccessStatusCode)
            {
                return ApiResponse<bool>.Fail("Không thể xóa bình luận");
            }
            return ApiResponse<bool>.Success(true);
        }
    }
}
