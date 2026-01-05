using Ecommerce.WebApi.ViewModels.AuthView;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Ecommerce.Web.Models
{
    public class LoginPageViewModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }
        //public LoginViewModel Login { get; set; } = new();
        //[ValidateNever]
        //public ExternalLoginViewModel ExternalLogin { get; set; } = new();
    }
}
