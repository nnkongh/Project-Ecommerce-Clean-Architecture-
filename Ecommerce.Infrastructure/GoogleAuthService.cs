using Ecommerce.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure
{
    //public class GoogleAuthService : IExternalAuthService
    //{
    //    private readonly string _clientId;
    //    private readonly string _clientSecret;
    //    private readonly HttpClient _httpClient;

    //    public GoogleAuthService(string clientId, string clientSecret, HttpClient httpClient)
    //    {
    //        _clientId = clientId;
    //        _clientSecret = clientSecret;
    //        _httpClient = httpClient;
    //    }
    //    public async Task<ExternalAuthResult> ExchangeCodeForTokenAsync(string code, string redirectUri)
    //    {
    //        try
    //        {
    //            var tokenRequest = new Dictionary<string, string>
    //            {
    //                { "code", code },
    //                { "client_id", _clientId },
    //                { "client_secret", _clientSecret },
    //                { "redirect_uri", redirectUri },
    //                { "grant_type", "authorization_code" }
    //            };
    //            var tokenResponse = await _httpClient.PostAsync("https://oauth2.googleapis.com/token", new FormUrlEncodedContent(tokenRequest));
    //            var tokenData = await tokenResponse.Content.ReadFromJsonAsync<GoogleTokenResponse>();

    //            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenData.AccessToken);
    //            var userInfoResponse = await _httpClient.GetAsync("https://www.googleapis.com/oauth2/v2/userinfo");

    //            if (!userInfoResponse.IsSuccessStatusCode)
    //            {
    //                return new ExternalAuthResult { IsSuccess = false };
    //            }
    //            var userInfo = await userInfoResponse.Content.ReadFromJsonAsync<GoogleUserInfoResponse>();
    //            return new ExternalAuthResult
    //            {
    //                IsSuccess = true,
    //                Email = userInfo.Email,
    //                Name = userInfo.Name,
    //                GoogleId = userInfo.Id,
    //                PictureUrl = userInfo.Picture
    //            };
    //        }
    //        catch (Exception ex)
    //        {
    //            return new ExternalAuthResult { IsSuccess = false };
    //        }
    //    }
    //}
}
