namespace Ecommerce.WebApi.ViewModels.AuthView
{
    public class ExternalLoginViewModel
    {
        public string Provider { get; set; }
        public string IdToken { get; set; }
        public string ReturnUrl { get; set; }
    }

}
