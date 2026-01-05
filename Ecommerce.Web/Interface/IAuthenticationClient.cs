using Ecommerce.Application.DTOs.Authentication;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Shared;
using Ecommerce.Web.ViewModels;
using Ecommerce.Web.ViewModels.ApiResponse;
using Ecommerce.WebApi.ViewModels.AuthView;

namespace Ecommerce.Web.Interface
{
    public interface IAuthenticationClient
    {
        Task<ApiResponse<TokenModel>> LoginAsync(LoginPageViewModel model);
        Task<ApiResponse<UserModel>> RegisterAsync(RegisterViewModel model);    
        Task<ApiResponse<string>> ForgotPasswordAsync(ForgotPasswordViewModel model);
        Task<ApiResponse<bool>> ResetPasswordAsync(ResetPasswordViewModel model);
        Task LogoutAsync();

    }
}
