using Ecommerce.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Interfaces
{
    public interface IUserAuthTokenService
    {
        Task<string?> GenerateTokenAsync(string userId);
        Task<bool> ResetPasswordAsync(string userId, string token, string password);
        Task<string> GenerateEmailConfirmationTokenAsync(string userId);
        Task<Result> ConfirmEmailAsync(string userId, string token);
    }
}
