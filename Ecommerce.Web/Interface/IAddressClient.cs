using Ecommerce.Application.DTOs.ModelsRequest.Address;
using Ecommerce.Web.ViewModels.ApiResponse;
using Ecommerce.Web.Models;

namespace Ecommerce.Web.Interface
{
    public interface IAddressClient
    {
        Task<ApiResponse<IReadOnlyList<UserAddressViewModel>>> GetAddressesAsync();
        Task<ApiResponse<UserAddressViewModel>> CreateAddressAsync(CreateAddressRequest request);
        Task<ApiResponse<UserAddressViewModel>> UpdateAddressAsync(UpdateAddressRequest request);
        Task<ApiResponse<bool>> DeleteAddressAsync(int id);
        Task<ApiResponse<bool>> SetDefaultAddressAsync(int id);
    }
}
