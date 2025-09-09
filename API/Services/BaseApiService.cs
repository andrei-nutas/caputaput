using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TAF_iSAMS.Test.API.Auth;
using TAF_iSAMS.Test.API.Config;

namespace TAF_iSAMS.Test.API.Services
{
    /// <summary>
    /// Base class for all API services
    /// </summary>
    public abstract class BaseApiService
    {
        protected readonly ApiConfig ApiConfig;
        protected readonly OAuthHandler OAuthHandler;
        protected readonly JsonSerializerOptions JsonOptions;

        protected BaseApiService(ApiConfig apiConfig, OAuthHandler oAuthHandler)
        {
            ApiConfig = apiConfig ?? throw new ArgumentNullException(nameof(apiConfig));
            OAuthHandler = oAuthHandler ?? throw new ArgumentNullException(nameof(oAuthHandler));

            JsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        /// <summary>
        /// Creates an HTTP client with the appropriate authorization header
        /// </summary>
        /// <returns>An HTTP client</returns>
        protected async Task<HttpClient> CreateHttpClientAsync()
        {
            var client = new HttpClient();
            var token = await OAuthHandler.GetBearerTokenAsync();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Add other headers that might be needed
            client.DefaultRequestHeaders.Add("Connection", "keep-alive");
            client.DefaultRequestHeaders.Add("User-Agent", "TAF_iSAMS Test Framework");

            return client;
        }

        /// <summary>
        /// Makes a GET request to the specified endpoint
        /// </summary>
        /// <typeparam name="T">The type to deserialize the response to</typeparam>
        /// <param name="endpoint">The endpoint to make the request to</param>
        /// <returns>The deserialized response</returns>
        protected async Task<T> GetAsync<T>(string endpoint)
        {
            using var client = await CreateHttpClientAsync();

            // Remove leading slash if present
            if (endpoint.StartsWith("/"))
            {
                endpoint = endpoint.Substring(1);
            }

            var requestUrl = $"{ApiConfig.BaseUrl}/{endpoint}";
            TestContext.WriteLine($"Making GET request to: {requestUrl}");
            TestContext.WriteLine($"Authorization: Bearer {client.DefaultRequestHeaders.Authorization?.Parameter?.Substring(0, Math.Min(10, client.DefaultRequestHeaders.Authorization?.Parameter?.Length ?? 0))}...");

            var response = await client.GetAsync(requestUrl);

            TestContext.WriteLine($"Response status: {(int)response.StatusCode} {response.StatusCode}");

            var content = await response.Content.ReadAsStringAsync();

            // Log the first part of the response content to debug the structure
            TestContext.WriteLine($"Response content starts with: {content.Substring(0, Math.Min(500, content.Length))}");

            if (!response.IsSuccessStatusCode)
            {
                TestContext.WriteLine($"Error response: {content}");
                response.EnsureSuccessStatusCode();
            }

            try
            {
                return JsonSerializer.Deserialize<T>(content, JsonOptions);
            }
            catch (JsonException ex)
            {
                TestContext.WriteLine($"JSON Deserialization error: {ex.Message}");
                TestContext.WriteLine($"Full JSON response: {content}");
                throw;
            }
        }

        /// <summary>
        /// Makes a POST request to the specified endpoint
        /// </summary>
        /// <typeparam name="TRequest">The type of the request body</typeparam>
        /// <typeparam name="TResponse">The type to deserialize the response to</typeparam>
        /// <param name="endpoint">The endpoint to make the request to</param>
        /// <param name="requestBody">The request body</param>
        /// <returns>The deserialized response</returns>
        protected async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest requestBody)
        {
            using var client = await CreateHttpClientAsync();

            // Remove leading slash if present
            if (endpoint.StartsWith("/"))
            {
                endpoint = endpoint.Substring(1);
            }

            var requestUrl = $"{ApiConfig.BaseUrl}/{endpoint}";
            TestContext.WriteLine($"Making POST request to: {requestUrl}");

            var content = new StringContent(
                JsonSerializer.Serialize(requestBody, JsonOptions),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync(requestUrl, content);

            TestContext.WriteLine($"Response status: {(int)response.StatusCode} {response.StatusCode}");

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                TestContext.WriteLine($"Error response: {errorContent}");
            }

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TResponse>(responseContent, JsonOptions);
        }

        /// <summary>
        /// Makes a POST request to the specified endpoint with no request body
        /// </summary>
        /// <typeparam name="TResponse">The type to deserialize the response to</typeparam>
        /// <param name="endpoint">The endpoint to make the request to</param>
        /// <returns>The deserialized response</returns>
        protected async Task<TResponse> PostAsync<TResponse>(string endpoint)
        {
            using var client = await CreateHttpClientAsync();

            // Remove leading slash if present
            if (endpoint.StartsWith("/"))
            {
                endpoint = endpoint.Substring(1);
            }

            var requestUrl = $"{ApiConfig.BaseUrl}/{endpoint}";
            TestContext.WriteLine($"Making POST request to: {requestUrl}");

            var response = await client.PostAsync(requestUrl, null);

            TestContext.WriteLine($"Response status: {(int)response.StatusCode} {response.StatusCode}");

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                TestContext.WriteLine($"Error response: {errorContent}");
            }

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TResponse>(responseContent, JsonOptions);
        }

        /// <summary>
        /// Makes a DELETE request to the specified endpoint
        /// </summary>
        /// <param name="endpoint">The endpoint to make the request to</param>
        /// <returns>True if the request was successful</returns>
        protected async Task<bool> DeleteAsync(string endpoint)
        {
            using var client = await CreateHttpClientAsync();

            // Remove leading slash if present
            if (endpoint.StartsWith("/"))
            {
                endpoint = endpoint.Substring(1);
            }

            var requestUrl = $"{ApiConfig.BaseUrl}/{endpoint}";
            TestContext.WriteLine($"Making DELETE request to: {requestUrl}");

            var response = await client.DeleteAsync(requestUrl);

            TestContext.WriteLine($"Response status: {(int)response.StatusCode} {response.StatusCode}");

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                TestContext.WriteLine($"Error response: {errorContent}");
            }

            return response.IsSuccessStatusCode;
        }
    }
}