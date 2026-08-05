using Ecommerce.Web.ViewModels;
using Ecommerce.Web.ViewModels.ApiResponse;

namespace Ecommerce.Web.Interface
{
    public interface IWishlistClient
    {
        Task<ApiResponse<WishlistViewModel?>> GetMyWishlistAsync();
        Task<ApiResponse<WishlistViewModel>> AddItemToWishlistAsync(int productId);
        Task<ApiResponse<bool>> RemoveItemFromWishlistAsync(int productId, int wishlistId);
        Task<ApiResponse<CartViewModel>> MoveItemToCartAsync(int wishlistId, int productId);
    }
}
