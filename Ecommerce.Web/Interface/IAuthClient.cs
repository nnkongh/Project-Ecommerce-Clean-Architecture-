using Ecommerce.Application.DTOs.Models;
using Ecommerce.Web.ViewModels.ApiResponse;
using Ecommerce.WebApi.ViewModels.AuthView;

namespace Ecommerce.Web.Interface
{
    public interface IAuthClient
    {
        Task<ApiResponse<LoginResponse>> LoginAsync(LoginViewModel model);
        Task<ApiResponse<UserModel>> RegisterAsync(RegisterViewModel model);
        //Task<ApiResponse<LoginResponse>> ExternalLoginAsync(ExternalLoginViewModel model);
        Task<ApiResponse<string>> ForgotPasswordAsync(ForgotPasswordViewModel model);
        Task<ApiResponse<bool>> ResetPasswordAsync(ResetPasswordViewModel model);
        Task LogoutAsync();
    }
}
