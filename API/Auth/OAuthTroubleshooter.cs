using System.Net.Http.Headers;
using System.Text;
using TAF_iSAMS.Test.TestData.Providers;

namespace TAF_iSAMS.Test.API.Auth
{
    /// <summary>
    /// A utility class for troubleshooting OAuth token endpoint issues
    /// </summary>
    public class OAuthTroubleshooter
    {
        private readonly string _tokenEndpoint;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _scope;

        /// <summary>
        /// Initializes a new instance of the OAuthTroubleshooter class using configured values
        /// </summary>
        public OAuthTroubleshooter()
        {
            // Get the configuration from the provider
            var apiConfig = TestConfigProvider.GetApiConfig();

            _tokenEndpoint = apiConfig.TokenEndpoint;
            _clientId = apiConfig.ClientId;
            _clientSecret = apiConfig.ClientSecret;
            _scope = apiConfig.Scope;
        }

        /// <summary>
        /// Initializes a new instance of the OAuthTroubleshooter class with explicit values
        /// </summary>
        /// <param name="tokenEndpoint">The OAuth token endpoint URL</param>
        /// <param name="clientId">The client ID</param>
        /// <param name="clientSecret">The client secret</param>
        /// <param name="scope">The requested scope</param>
        public OAuthTroubleshooter(string tokenEndpoint, string clientId, string clientSecret, string scope)
        {
            _tokenEndpoint = tokenEndpoint;
            _clientId = clientId;
            _clientSecret = clientSecret;
            _scope = scope;
        }

        /// <summary>
        /// Performs direct troubleshooting of the OAuth token endpoint using multiple approaches
        /// </summary>
        /// <returns>A task representing the asynchronous operation</returns>
        public async Task DirectOAuthTest()
        {
            // Setup
            TestContext.WriteLine($"=== OAUTH TROUBLESHOOTING ===");
            TestContext.WriteLine($"Endpoint: {_tokenEndpoint}");
            TestContext.WriteLine($"Client ID: {_clientId}");
            TestContext.WriteLine($"Scope: {_scope}");

            // Create the HTTP client
            using var client = new HttpClient();

            // Approach 1: Using StringContent directly
            TestContext.WriteLine("\nTEST 1: Using StringContent");

            try
            {
                var formData = new StringContent(
                    $"client_id={Uri.EscapeDataString(_clientId)}" +
                    $"&client_secret={Uri.EscapeDataString(_clientSecret)}" +
                    $"&grant_type=client_credentials" +
                    $"&scope={Uri.EscapeDataString(_scope)}",
                    Encoding.UTF8,
                    "application/x-www-form-urlencoded");

                var request = new HttpRequestMessage(HttpMethod.Post, _tokenEndpoint);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Content = formData;

                TestContext.WriteLine($"Content-Type: {formData.Headers.ContentType}");

                // Send the request
                var response = await client.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();

                TestContext.WriteLine($"Status: {(int)response.StatusCode} {response.StatusCode}");

                // Mask the token for security
                if (response.IsSuccessStatusCode && content.Contains("access_token"))
                {
                    var truncatedContent = MaskToken(content);
                    TestContext.WriteLine($"Response: {truncatedContent}");
                }
                else
                {
                    TestContext.WriteLine($"Response: {content}");
                }

                Assert.That(response.IsSuccessStatusCode, Is.True, "Request should succeed");
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"ERROR: {ex.Message}");
                Assert.Fail($"Exception occurred: {ex.Message}");
            }

            // Approach 2: Using FormUrlEncodedContent 
            TestContext.WriteLine("\nTEST 2: Using FormUrlEncodedContent");

            try
            {
                var formValues = new[]
                {
                    new KeyValuePair<string, string>("client_id", _clientId),
                    new KeyValuePair<string, string>("client_secret", _clientSecret),
                    new KeyValuePair<string, string>("grant_type", "client_credentials"),
                    new KeyValuePair<string, string>("scope", _scope)
                };

                var formContent = new FormUrlEncodedContent(formValues);

                var request = new HttpRequestMessage(HttpMethod.Post, _tokenEndpoint);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Content = formContent;

                // Send the request
                var response = await client.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();

                TestContext.WriteLine($"Status: {(int)response.StatusCode} {response.StatusCode}");

                // Mask the token for security
                if (response.IsSuccessStatusCode && content.Contains("access_token"))
                {
                    var truncatedContent = MaskToken(content);
                    TestContext.WriteLine($"Response: {truncatedContent}");
                }
                else
                {
                    TestContext.WriteLine($"Response: {content}");
                }

                Assert.That(response.IsSuccessStatusCode, Is.True, "Request should succeed");
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"ERROR: {ex.Message}");
                Assert.Fail($"Exception occurred: {ex.Message}");
            }

            // Approach 3: Using HttpClient.PostAsync directly
            TestContext.WriteLine("\nTEST 3: Using PostAsync directly");

            try
            {
                var formValues = new Dictionary<string, string>
                {
                    { "client_id", _clientId },
                    { "client_secret", _clientSecret },
                    { "grant_type", "client_credentials" },
                    { "scope", _scope }
                };

                var formContent = new FormUrlEncodedContent(formValues);

                // Set Accept header on the client
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Send the request
                var response = await client.PostAsync(_tokenEndpoint, formContent);
                var content = await response.Content.ReadAsStringAsync();

                TestContext.WriteLine($"Status: {(int)response.StatusCode} {response.StatusCode}");

                // Mask the token for security
                if (response.IsSuccessStatusCode && content.Contains("access_token"))
                {
                    var truncatedContent = MaskToken(content);
                    TestContext.WriteLine($"Response: {truncatedContent}");
                }
                else
                {
                    TestContext.WriteLine($"Response: {content}");
                }

                Assert.That(response.IsSuccessStatusCode, Is.True, "Request should succeed");
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"ERROR: {ex.Message}");
                Assert.Fail($"Exception occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Masks the token in the response for security
        /// </summary>
        /// <param name="content">The response content</param>
        /// <returns>The masked content</returns>
        private string MaskToken(string content)
        {
            // This is a simple approach - in a real implementation, you might want to use JSON parsing
            if (content.Contains("access_token"))
            {
                int startIndex = content.IndexOf("access_token") + "access_token".Length + 3; // +3 for "":"
                int endIndex = content.IndexOf("\"", startIndex);

                if (startIndex > 0 && endIndex > startIndex)
                {
                    string token = content.Substring(startIndex, endIndex - startIndex);
                    string maskedToken = token.Substring(0, Math.Min(10, token.Length)) + "..." +
                                       (token.Length > 5 ? token.Substring(token.Length - 5) : "");

                    return content.Replace(token, maskedToken);
                }
            }

            return content;
        }
    }
}