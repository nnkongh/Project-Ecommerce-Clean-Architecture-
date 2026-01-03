using Ecommerce.WebApi.ViewModels.AuthView;

namespace Ecommerce.Web.Models
{
    public class LoginPageViewModel
    {
        public LoginViewModel Login { get; set; } = new();
        public ExternalLoginViewModel ExternalLogin { get; set; } = new();
    }
}
