using Ecommerce.Application.DTOs.Authentication;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Enum;
using Ecommerce.Infrastructure.Exceptions;
using Google.Apis.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Services.ExternalAuth
{
    public class GoogleAuthProvider : IExternalAuthProvider
    {
        public ProviderType ProviderType => ProviderType.Google;

        public async Task<ExternalUserInfo> ValidateAndGetUserInfoAsync(string token)
        {
            // Get Payload
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(token);
                return new ExternalUserInfo(payload.Subject, payload.Email, payload.Name);
            }
            catch (InvalidJwtException)
            {
                throw new AuthenticationException("invalid google token");
            }
        }
    }
}
