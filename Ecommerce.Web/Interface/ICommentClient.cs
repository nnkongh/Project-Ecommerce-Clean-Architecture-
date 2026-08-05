using Ecommerce.Web.ViewModels;
using Ecommerce.Web.ViewModels.ApiResponse;

namespace Ecommerce.Web.Interface
{
    public interface ICommentClient
    {
        Task<ApiResponse<IReadOnlyList<CommentViewModel>>> GetCommentsByProductIdAsync(int productId);
        Task<ApiResponse<CommentViewModel>> CreateCommentAsync(int productId, string content);
        Task<ApiResponse<bool>> DeleteCommentAsync(int commentId);
    }
}
