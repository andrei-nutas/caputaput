using System.Text.Json.Serialization;

namespace TAF_iSAMS.Test.API.Models.Auth
{
    /// <summary>
    /// Represents the response from the OAuth token endpoint
    /// </summary>
    public class TokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        [JsonPropertyName("scope")]
        public string Scope { get; set; }
    }
}