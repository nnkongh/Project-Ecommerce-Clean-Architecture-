using System.Text.Json.Serialization;

namespace Ecommerce.Application.DTOs.Authentication
{
    public class TokenModel
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = default!;
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; } = default!;
    }
}