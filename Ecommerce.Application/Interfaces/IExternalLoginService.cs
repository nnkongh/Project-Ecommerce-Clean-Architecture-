using Ecommerce.Application.DTOs.Authentication;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Enum;
using Ecommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Interfaces
{
    public interface IExternalLoginService
    {
        Task<UserModel?> FindByExternalLoginAsync(string provider, string providerKey);
        Task AddExternalLoginToExistingUserAsync(string userId, ExternalIdentity externalIdentity);
        Task<UserModel> CreateUserFromExternalAsync(ExternalUserInfo externalUserInfo, ExternalIdentity externalIdenitty, CancellationToken cancellationToken);
    }
    public class ExternalIdentity
    {
        public string Provider { get; set; }
        public string ProviderKey { get; set; }
    }
}
