namespace Ecommerce.Infrastructure
{
    internal class GoogleTokenResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string ExpiresIn { get; set; }
    }
}