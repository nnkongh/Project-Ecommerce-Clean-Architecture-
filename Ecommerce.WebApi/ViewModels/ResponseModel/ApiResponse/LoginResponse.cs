namespace Ecommerce.Web.ViewModels.ApiResponse
{
    public class LoginResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
