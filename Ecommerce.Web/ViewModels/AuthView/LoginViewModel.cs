using System.ComponentModel.DataAnnotations;

namespace Ecommerce.WebApi.ViewModels.AuthView
{
    public class LoginViewModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }

    }
}
