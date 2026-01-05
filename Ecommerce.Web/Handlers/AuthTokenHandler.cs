using Ecommerce.Web.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure
{
    public class AuthTokenHandler : DelegatingHandler
    {
        private readonly ICookieTokenService _cookieTokenService;

        public AuthTokenHandler(ICookieTokenService cookieTokenService)
        {
            _cookieTokenService = cookieTokenService;
        }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancenllationToken) {
            var token = _cookieTokenService.GetAccessToken();
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            return base.SendAsync(request, cancenllationToken);
        }
    }
}
