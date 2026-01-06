using Ecommerce.Application.DTOs.Models;
using Ecommerce.Application.DTOs.ModelsRequest.User;
using Ecommerce.Domain.Shared;
using Ecommerce.Web.ViewModels.ApiResponse;
using Ecommerce.Web.ViewModels.Profile;

namespace Ecommerce.Web.Interface
{
    public interface IProfileClient
    {
        Task<ApiResponse<ProfileViewModel>> GetProfileAsync();
        Task<ApiResponse<UpdateProfileRequest>> GetProfileForEditAsync();
        Task<ApiResponse<ProfileViewModel>> UpdateProfileAsync(UpdateProfileRequest request);
        
    }
}
