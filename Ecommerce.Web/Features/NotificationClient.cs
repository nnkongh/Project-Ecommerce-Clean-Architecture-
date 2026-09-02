using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Web.Interface;
using Ecommerce.Web.ViewModels;
using Ecommerce.Web.ViewModels.ApiResponse;

namespace Ecommerce.Web.Services
{
    public class NotificationClient : INotificationClient
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public NotificationClient(IHttpClientFactory httpClient, IMapper mapper)
        {
            _mapper = mapper;
            _httpClient = httpClient.CreateClient("ApiClient");
        }

        public async Task<ApiResponse<List<NotificationViewModel>>> GetMyNotificationsAsync()
        {
            var response = await _httpClient.GetAsync("notification/my");
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<NotificationModel>>>();

            if (result == null || !result.IsSuccess)
            {
                return ApiResponse<List<NotificationViewModel>>.Fail(result?.Error?.Message ?? "Không thể lấy thông báo");
            }

            var mapped = _mapper.Map<List<NotificationViewModel>>(result.Value);
            return ApiResponse<List<NotificationViewModel>>.Success(mapped);
        }

        public async Task<ApiResponse<int>> GetUnreadCountAsync()
        {
            var response = await _httpClient.GetAsync("notification/unread-count");
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<int>>();

            if (result == null || !result.IsSuccess)
            {
                return ApiResponse<int>.Fail(result?.Error?.Message ?? "Không thể lấy số thông báo chưa đọc");
            }

            return ApiResponse<int>.Success(result.Value);
        }

        public async Task<ApiResponse<bool>> MarkAsReadAsync(int? id = null)
        {
            var url = id.HasValue ? $"notification/mark-read/{id.Value}" : "notification/mark-read";
            var response = await _httpClient.PostAsync(url, null);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();

            if (result == null || !result.IsSuccess)
            {
                return ApiResponse<bool>.Fail(result?.Error?.Message ?? "Không thể cập nhật thông báo");
            }

            return ApiResponse<bool>.Success(true);
        }
    }
}
