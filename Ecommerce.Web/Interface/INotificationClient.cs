using Ecommerce.Web.ViewModels;
using Ecommerce.Web.ViewModels.ApiResponse;

namespace Ecommerce.Web.Interface
{
    public interface INotificationClient
    {
        Task<ApiResponse<List<NotificationViewModel>>> GetMyNotificationsAsync();
        Task<ApiResponse<int>> GetUnreadCountAsync();
        Task<ApiResponse<bool>> MarkAsReadAsync(int? id = null);
    }
}
