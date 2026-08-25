using Ecommerce.Application.DTOs.Authentication;
using Ecommerce.Web.Exceptions;
using Ecommerce.Web.Interface;
using Microsoft.Extensions.Configuration;

namespace Ecommerce.Web.Handlers
{
    public class AuthTokenHandler : DelegatingHandler
    {
        private readonly ICookieTokenService _cookieTokenService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiBaseUrl;

        private static readonly string[] _authEndpoints =
        {
            "auth/login",
            "auth/register",
            "auth/forgot-password",
            "auth/reset-password",
            "auth/logout",
            "auth/refresh",
            "token/create-token"
        };

        public AuthTokenHandler(ICookieTokenService cookieTokenService, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _cookieTokenService = cookieTokenService;
            _httpClientFactory = httpClientFactory;
            _apiBaseUrl = configuration["ApiSettings:BaseUrl"]!;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = _cookieTokenService.GetAccessToken();

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized && !IsAuthEndpoint(request.RequestUri))
            {
                var refreshedToken = await TryRefreshTokenAsync(cancellationToken);

                if (string.IsNullOrEmpty(refreshedToken))
                {
                    throw new UnauthorizedException();
                }

                using var retryRequest = await CloneRequestAsync(request);
                retryRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", refreshedToken);
                response.Dispose();
                response = await base.SendAsync(retryRequest, cancellationToken);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedException();
                }
            }

            return response;
        }

        private async Task<string?> TryRefreshTokenAsync(CancellationToken cancellationToken)
        {
            try
            {
                var accessToken = _cookieTokenService.GetAccessToken();
                var refreshToken = _cookieTokenService.GetRefreshToken();

                if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
                {
                    return null;
                }

                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_apiBaseUrl);

                var response = await client.PostAsJsonAsync("auth/refresh", new TokenModel
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                }, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var payload = await response.Content.ReadFromJsonAsync<Ecommerce.Web.ViewModels.ApiResponse.ApiResponse<TokenModel>>(cancellationToken);

                if (payload?.Value == null || string.IsNullOrEmpty(payload.Value.AccessToken))
                {
                    return null;
                }

                _cookieTokenService.SetTokenInsideCookie(payload.Value);
                return payload.Value.AccessToken;
            }
            catch
            {
                return null;
            }
        }

        private static async Task<HttpRequestMessage> CloneRequestAsync(HttpRequestMessage request)
        {
            var clone = new HttpRequestMessage(request.Method, request.RequestUri)
            {
                Version = request.Version
            };

            if (request.Content != null)
            {
                var memoryStream = new MemoryStream();
                await request.Content.CopyToAsync(memoryStream);
                memoryStream.Position = 0;
                clone.Content = new StreamContent(memoryStream);

                foreach (var header in request.Content.Headers)
                {
                    clone.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            foreach (var header in request.Headers)
            {
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return clone;
        }

        private static bool IsAuthEndpoint(Uri? uri)
        {
            if (uri == null) return false;
            var path = uri.AbsolutePath.TrimStart('/').ToLowerInvariant();
            return _authEndpoints.Any(e => path.Contains(e));
        }
    }
}
