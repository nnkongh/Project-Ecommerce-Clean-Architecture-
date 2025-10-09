using Ecommerce.Application.DTOs.Authentication;
using Ecommerce.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Interfaces
{
    public interface IExternalAuthProvider
    {
        ProviderType ProviderType { get; }
        Task<ExternalUserInfo> ValidateAndGetUserInfoAsync(string token);
    }
}
