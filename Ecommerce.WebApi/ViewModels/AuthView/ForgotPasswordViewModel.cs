using System.ComponentModel.DataAnnotations;

namespace Ecommerce.WebApi.ViewModels.AuthView
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }
    }
}
