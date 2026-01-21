using AutoMapper;
using Ecommerce.Application.DTOs.Authentication;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Shared;
using Ecommerce.Web.Interface;
using Ecommerce.Web.ViewModels;
using Ecommerce.Web.ViewModels.ApiResponse;
using Ecommerce.WebApi.ViewModels.AuthView;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Ecommerce.Web.Features.Authentication
{
    public class AuthenticationClient : IAuthenticationClient
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        public AuthenticationClient(IMapper mapper, IHttpClientFactory httpClientFactory)
        {
            _mapper = mapper;
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }


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
                //if(!response.IsSuccessStatusCode)
                //{
                //    return 
                //}
                return await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.Fail("Failed");
            }
        }

        public async Task<ApiResponse<TokenModel>> LoginAsync(LoginPageViewModel model)
        {
            try
            {
                var request = new LoginModel
                {
                    Email = model.Email,
                    Password = model.Password,
                };
                var response = await _httpClient.PostAsJsonAsync("auth/login", request);
                return await response.Content.ReadFromJsonAsync<ApiResponse<TokenModel>>();
            }
            catch (Exception ex)
            {
                return ApiResponse<TokenModel>.Fail("Failed"); 
            }
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
                return ApiResponse<UserModel>.Fail("Failed");
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
                return ApiResponse<bool>.Fail("Failed");
            }
        }
    }
}