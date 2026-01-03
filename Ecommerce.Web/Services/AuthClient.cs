using Ecommerce.Application.DTOs.Authentication;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Web.Interface;
using Ecommerce.Web.ViewModels.ApiResponse;
using Ecommerce.WebApi.ViewModels.AuthView;

namespace Ecommerce.Web.Services
{
    public class AuthClient : IAuthClient
    {
        private readonly HttpClient _httpClient;

        public AuthClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        //public async Task<ApiResponse<LoginResponse>> ExternalLoginAsync(ExternalLoginViewModel model)
        //{
        //    try
        //    {
        //        var request = new ExternalLoginModel
        //        {
        //            ProviderType = model.Provider.ToString(),
        //            Token = model.IdToken,
        //        };
        //        var response = await _httpClient.PostAsJsonAsync("auth/login-external", request);
        //        return await response.Content.ReadFromJsonAsync<ApiResponse<LoginResponse>>();
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ApiResponse<LoginResponse>
        //        {
        //            IsSuccess = false,
        //            Error = new ApiError { Message = ex.Message }
        //        };
        //    }
        //}

        public async Task<ApiResponse<string>> ForgotPasswordAsync(ForgotPasswordViewModel model)
        {
            try
            {
                var request = new ForgotPasswordModel
                {
                    Email = model.Email,
                    ClientUrl = model.ClientUrl
                };
                var response = await _httpClient.PostAsJsonAsync("auth/forgot-password", request);
                return await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>
                {
                    IsSuccess = false,
                    Error = new ApiError { Message = ex.Message }
                };
            }
        }

        public async Task<ApiResponse<LoginResponse>> LoginAsync(LoginViewModel model)
        {
            try
            {
                var request = new LoginModel
                {
                    Email = model.Email,
                    Password = model.Password
                };
                var response = await _httpClient.PostAsJsonAsync("auth/login", request);
                return await response.Content.ReadFromJsonAsync<ApiResponse<LoginResponse>>();
            }
            catch (Exception ex)
            {
                return new ApiResponse<LoginResponse>
                {
                    IsSuccess = false,
                    Error = new ApiError { Message = ex.Message },
                    
                };
            }
        }

        public async Task LogoutAsync()
        {
            await _httpClient.GetAsync("auth/logout");
        }

        public async Task<ApiResponse<UserModel>> RegisterAsync(RegisterViewModel model)
        {
            try
            {
                var request = new RegisterModel
                {
                    Email = model.Email,
                    UserName = model.Email,
                    Password = model.Password,
                    ConfirmPassword = model.ConfirmPassword
                };
                var response = await _httpClient.PostAsJsonAsync("auth/register", request);
                return await response.Content.ReadFromJsonAsync<ApiResponse<UserModel>>();
            }
            catch (Exception ex)
            {
                return new ApiResponse<UserModel>
                {
                    IsSuccess = false,
                    Error = new ApiError { Message = ex.Message }
                };
            }
        }

        public async Task<ApiResponse<bool>> ResetPasswordAsync(ResetPasswordViewModel model)
        {
            try
            {
                var request = new ResetPasswordModel
                {
                    Email = model.Email,
                    Token = model.Token,
                    NewPassword = model.NewPassword,
                    ConfirmPassword = model.ConfirmPassword
                };
                var response = await _httpClient.PostAsJsonAsync("auth/reset-password", request);
                return await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();
            }

            catch (Exception ex)
            {
                return new ApiResponse<bool>
                {
                    IsSuccess = false,
                    Error = new ApiError { Message = ex.Message }
                };
            }
        }
    }
}