using System.Net.Http.Headers;
using System.Text.Json;
using TAF_iSAMS.Test.API.Config;
using TAF_iSAMS.Test.API.Models.Auth;

namespace TAF_iSAMS.Test.API.Auth
{
    /// <summary>
    /// Handles OAuth authentication and token management
    /// </summary>
    public class OAuthHandler
    {
        private readonly ApiConfig _apiConfig;
        private TokenResponse _currentToken;
        private DateTime _tokenExpiryTime;

        public OAuthHandler(ApiConfig apiConfig)
        {
            _apiConfig = apiConfig ?? throw new ArgumentNullException(nameof(apiConfig));
            _currentToken = null;
            _tokenExpiryTime = DateTime.MinValue;
        }

        /// <summary>
        /// Gets a valid OAuth bearer token, retrieving a new one if necessary
        /// </summary>
        /// <returns>The bearer token string</returns>
        public async Task<string> GetBearerTokenAsync()
        {
            // If we have a token and it's not expired, return it
            if (_currentToken != null && DateTime.UtcNow < _tokenExpiryTime)
            {
                TestContext.WriteLine($"Using existing token valid until {_tokenExpiryTime}");
                return _currentToken.AccessToken;
            }

            // Otherwise, get a new token
            TestContext.WriteLine("Getting new OAuth token...");
            _currentToken = await GetNewTokenAsync();

            // Set expiry time (subtract 30 seconds for safety margin)
            _tokenExpiryTime = DateTime.UtcNow.AddSeconds(_currentToken.ExpiresIn - 30);
            TestContext.WriteLine($"New token obtained, valid until {_tokenExpiryTime}");

            return _currentToken.AccessToken;
        }

        /// <summary>
        /// Gets a new OAuth token from the token endpoint
        /// </summary>
        /// <returns>The token response</returns>
        private async Task<TokenResponse> GetNewTokenAsync()
        {
            using var client = new HttpClient();

            // Create the request content using FormUrlEncodedContent
            var formValues = new[]
            {
                new KeyValuePair<string, string>("client_id", _apiConfig.ClientId),
                new KeyValuePair<string, string>("client_secret", _apiConfig.ClientSecret),
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("scope", _apiConfig.Scope)
            };

            var formContent = new FormUrlEncodedContent(formValues);

            // Set up the request - use the complete TokenEndpoint URL directly
            TestContext.WriteLine($"Requesting OAuth token from: {_apiConfig.TokenEndpoint}");
            var request = new HttpRequestMessage(HttpMethod.Post, _apiConfig.TokenEndpoint);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = formContent;

            // Add user agent header
            request.Headers.Add("User-Agent", "TAF_iSAMS Test Framework");

            // Send the request
            var response = await client.SendAsync(request);

            TestContext.WriteLine($"Token request status: {(int)response.StatusCode} {response.StatusCode}");

            // Throw an exception if the request was not successful
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                TestContext.WriteLine($"Token request error: {errorContent}");
                response.EnsureSuccessStatusCode(); // This will throw
            }

            // Parse the response
            var content = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            TestContext.WriteLine($"Successfully obtained token: {tokenResponse.AccessToken.Substring(0, Math.Min(10, tokenResponse.AccessToken.Length))}...");

            return tokenResponse;
        }
    }
}