using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Ecommerce.WebApi.ViewModels.AuthView
{
    public class ForgotPasswordViewModel
    {
        public string Email { get; set; }
        public string ClientUrl { get; set; }
    }
}
