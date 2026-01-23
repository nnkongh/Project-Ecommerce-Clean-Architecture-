using Ecommerce.Application.DTOs.Models;
using Ecommerce.Web.ViewModels;
using Ecommerce.Web.ViewModels.ApiResponse;

namespace Ecommerce.Web.Interface
{
    public interface ICheckoutCartClient
    {
        Task<ApiResponse<OrderViewModel>> CheckoutCartAsync();
    }
}
