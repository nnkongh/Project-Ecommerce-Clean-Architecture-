using Ecommerce.Web.ViewModels;
using Ecommerce.Web.ViewModels.ApiResponse;

namespace Ecommerce.Web.Interface
{
    public interface IReviewClient
    {
        Task<ApiResponse<IReadOnlyList<ReviewViewModel>>> GetReviewsByProductIdAsync(int productId);
        Task<ApiResponse<ReviewViewModel>> CreateReviewAsync(int productId, string content, int rating);
        Task<ApiResponse<bool>> DeleteReviewAsync(int reviewId);
    }
}
