namespace TAF_iSAMS.Test.API.Config
{
    /// <summary>
    /// Configuration for the API endpoints and authentication
    /// </summary>
    public class ApiConfig
    {
        /// <summary>
        /// Base URL for all API endpoints
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Full OAuth token endpoint URL
        /// </summary>
        public string TokenEndpoint { get; set; }

        /// <summary>
        /// OAuth client ID
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// OAuth client secret
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// OAuth scope
        /// </summary>
        public string Scope { get; set; }
    }
}