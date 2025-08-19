using System.Text.Json;
using TAF_iSAMS.Test.API.Config;

namespace TAF_iSAMS.Test.TestData.Providers
{
    /// <summary>
    /// Provides configuration for tests
    /// </summary>
    public class TestConfigProvider
    {
        private const string SettingsFilePath = "appsettings.json";
        private static ApiConfig _apiConfig;

        /// <summary>
        /// Gets the API configuration
        /// </summary>
        /// <returns>The API configuration</returns>
        public static ApiConfig GetApiConfig()
        {
            if (_apiConfig != null)
            {
                return _apiConfig;
            }

            try
            {
                // Parse configuration from settings file
                var json = File.ReadAllText(SettingsFilePath);
                var config = JsonSerializer.Deserialize<TestSettings>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                _apiConfig = config.ApiConfig;

                // If we're using environment variables, override with those
                if (config.UseEnvironmentVariables)
                {
                    _apiConfig.BaseUrl = Environment.GetEnvironmentVariable("API_BASE_URL") ?? _apiConfig.BaseUrl;
                    _apiConfig.TokenEndpoint = Environment.GetEnvironmentVariable("API_TOKEN_ENDPOINT") ?? _apiConfig.TokenEndpoint;
                    _apiConfig.ClientId = Environment.GetEnvironmentVariable("API_CLIENT_ID") ?? _apiConfig.ClientId;
                    _apiConfig.ClientSecret = Environment.GetEnvironmentVariable("API_CLIENT_SECRET") ?? _apiConfig.ClientSecret;
                    _apiConfig.Scope = Environment.GetEnvironmentVariable("API_SCOPE") ?? _apiConfig.Scope;
                }

                return _apiConfig;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to load API configuration: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Settings loaded from the configuration file
        /// </summary>
        private class TestSettings
        {
            public ApiConfig ApiConfig { get; set; }
            public bool UseEnvironmentVariables { get; set; }
        }
    }
}