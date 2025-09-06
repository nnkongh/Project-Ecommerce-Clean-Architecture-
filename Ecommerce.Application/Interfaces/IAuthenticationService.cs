using Ecommerce.Application.DTOs;
using Ecommerce.Application.DTOs.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<TokenModel> Login(LoginModel model);
        Task<UserModel> Register(RegisterModel model);
        Task ForgotPassword(ForgotPasswordModel model);
        Task ResetPassword(ResetPasswordModel model);

    }
}
