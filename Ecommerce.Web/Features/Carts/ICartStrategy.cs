using Ecommerce.Application.DTOs.ModelsRequest.Cart;
using Ecommerce.Web.ViewModels;

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
    }
}
