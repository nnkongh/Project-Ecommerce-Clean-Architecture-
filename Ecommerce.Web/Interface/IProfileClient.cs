using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Shared;
using Ecommerce.Web.ViewModels.ApiResponse;
using Ecommerce.Web.ViewModels.Profile;

namespace Ecommerce.Web.Interface
{
    public interface IProfileClient
    {
        Task<ApiResponse<ProfileViewModel>> GetProfileAsync();
        
    }
}
