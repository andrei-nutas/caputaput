using TAF_iSAMS.Test.API.Auth;
using TAF_iSAMS.Test.API.Config;
using TAF_iSAMS.Test.API.Models.School;

namespace TAF_iSAMS.Test.API.Services.School
{
    /// <summary>
    /// Service for interacting with term endpoints
    /// </summary>
    public class TermService : BaseApiService
    {
        public TermService(ApiConfig apiConfig, OAuthHandler oAuthHandler)
            : base(apiConfig, oAuthHandler)
        {
        }

        /// <summary>
        /// Gets all terms
        /// </summary>
        /// <returns>A list of terms</returns>
        public async Task<List<Term>> GetAllTermsAsync()
        {
            // Get the wrapped response and return just the terms
            var response = await GetAsync<TermResponse>("school/terms");
            return response.Terms;
        }
    }
}