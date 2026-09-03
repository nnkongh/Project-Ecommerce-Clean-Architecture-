
using Ecommerce.Application.DTOs.ModelsRequest.Order;
using Ecommerce.Web.ViewModels;
using Ecommerce.Web.ViewModels.ApiResponse;

namespace Ecommerce.Web.Interface
{
    public interface IOrderClient
    {
        Task<ApiResponse<OrderViewModel>> CreatOrderAsync(CreateOrderRequest request);
        Task<ApiResponse<IReadOnlyList<OrderViewModel>>> GetListOrderAsync();
        Task<ApiResponse<IReadOnlyList<OrderViewModel>>> GetOrdersByShopAsync();
        Task<ApiResponse<OrderViewModel>> GetOrderByIdAsync(int id);
        Task<ApiResponse<bool>> UpdateOrderStatusAsync(int orderId);
        Task<ApiResponse<bool>> RejectOrderAsync(int orderId);
    }
}
