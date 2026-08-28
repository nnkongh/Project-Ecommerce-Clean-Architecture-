using Ecommerce.Web.ViewModels;
using Ecommerce.Web.ViewModels.ApiResponse;

namespace Ecommerce.Web.Interface
{
    public interface IShopClient
    {
        Task<ApiResponse<ShopViewModel>> GetMyShopAsync();
        Task<ApiResponse<ShopViewModel>> CreateShopAsync(string name);
        Task<ApiResponse<ShopViewModel>> UpdateShopAsync(int id, string name);
        Task<ApiResponse<bool>> DeleteShopAsync(int id);
    }
}
