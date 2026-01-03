using Ecommerce.Application.Common.Command.AuthenticationExternal;
using Ecommerce.Application.DTOs.Authentication;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Models;
using Ecommerce.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Interfaces
{
    public interface IAuthService
    {
        Task<Result<UserModel>> Login(LoginModel model);
        Task<Result<UserModel>> Register(RegisterModel model);
        Task<Result> ForgotPassword(ForgotPasswordModel model); 
        Task<Result> ResetPassword(ResetPasswordModel model);
        //Task<ExternalLoginResult> LoginExternal(ExternalUserInfo model);

    }
}
