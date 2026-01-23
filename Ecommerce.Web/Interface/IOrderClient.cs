
using Ecommerce.Web.ViewModels;
using Ecommerce.Web.ViewModels.ApiResponse;

namespace Ecommerce.Web.Interface
{
    public interface IOrderClient
    {
        Task<ApiResponse<OrderViewModel>> CreatOrderAsync();
        Task<ApiResponse<IReadOnlyList<OrderViewModel>>> GetListOrderAsync();
        Task<ApiResponse<OrderViewModel>> GetOrderByIdAsync(int id);
    }
}
