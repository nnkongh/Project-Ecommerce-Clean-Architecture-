using Ecommerce.Web.Exceptions;
using Ecommerce.Web.Interface;

namespace Ecommerce.Web.Handlers
{
    public class AuthTokenHandler : DelegatingHandler
    {
        private readonly ICookieTokenService _cookieTokenService;

        private static readonly string[] _authEndpoints =
        {
            "auth/login",
            "auth/register",
            "auth/forgot-password",
            "auth/reset-password",
            "token/create-token"
        };

        public AuthTokenHandler(ICookieTokenService cookieTokenService)
        {
            _cookieTokenService = cookieTokenService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = _cookieTokenService.GetAccessToken();

            if (string.IsNullOrEmpty(token) && !IsAuthEndpoint(request.RequestUri))
            {
                throw new UnauthorizedException();
            }

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized && !IsAuthEndpoint(request.RequestUri))
            {
                throw new UnauthorizedException();
            }

            return response;
        }

        private static bool IsAuthEndpoint(Uri? uri)
        {
            if (uri == null) return false;
            var path = uri.AbsolutePath.TrimStart('/').ToLowerInvariant();
            return _authEndpoints.Any(e => path.Contains(e));
        }
    }
}
