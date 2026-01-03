using Ecommerce.Application.DTOs.Authentication;
using Ecommerce.Application.DTOs.Models;
using MediatR;

namespace Ecommerce.Application.Common.Command.AuthenticationExternal
{
    public sealed class ExternalLoginCommand : IRequest<ExternalLoginResult>
    {
        public ExternalUserInfo info { get; set; }
        //public string? RedirectUri { get; set; }
    }

    public class ExternalLoginResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public UserModel? User { get; set; }
        public TokenModel? Token { get; set; }
    }
}
