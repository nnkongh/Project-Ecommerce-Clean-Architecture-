using Ecommerce.Domain.Enum;

namespace Ecommerce.WebApi.ViewModels.AuthView
{
    public class ExternalLoginViewModel
    {
        public string Provider { get; set; }
        public string RedirectUri { get; set; }
    }

}
