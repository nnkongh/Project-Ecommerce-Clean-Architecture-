using Ecommerce.Application.DTOs.ModelsRequest.Carts;
using Ecommerce.Web.ViewModels;
using Ecommerce.Web.ViewModels.ApiResponse;

namespace Ecommerce.Web.Services.Strategy
{
    public interface ICartStrategy
    {
        // Định nghĩa các phương thức hỗ trợ 
        Task<CartViewModel> GetCartAsync();
        Task AddToCartAsync(AddToCartRequest model);
        Task RemoveFromCartAsync(int productId);
        Task ClearCartAsync();
        Task UpdateCartAsync(int productId, int quantity);
        Task<ApiResponse<OrderViewModel>> CheckOutCart();
    }
}
