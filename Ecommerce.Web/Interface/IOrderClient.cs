
using Ecommerce.Web.ViewModels;
using Ecommerce.Web.ViewModels.ApiResponse;

namespace Ecommerce.Web.Interface
{
    public interface IOrderClient
    {
        Task<ApiResponse<OrderViewModel>> CreatOrderAsync();
    }
}
