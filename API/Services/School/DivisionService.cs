using TAF_iSAMS.Test.API.Auth;
using TAF_iSAMS.Test.API.Config;
using TAF_iSAMS.Test.API.Models.School;

namespace TAF_iSAMS.Test.API.Services.School
{
    /// <summary>
    /// Service for interacting with division endpoints
    /// </summary>
    public class DivisionService : BaseApiService
    {
        public DivisionService(ApiConfig apiConfig, OAuthHandler oAuthHandler)
            : base(apiConfig, oAuthHandler)
        {
        }

        /// <summary>
        /// Gets all divisions
        /// </summary>
        /// <returns>A list of divisions</returns>
        public async Task<List<Division>> GetAllDivisionsAsync()
        {
            var response = await GetAsync<DivisionResponse>("school/divisions");
            return response.Divisions;
        }

        /// <summary>
        /// Gets a division by code
        /// </summary>
        /// <param name="code">The division code</param>
        /// <returns>The division</returns>
        public async Task<Division> GetDivisionByCodeAsync(string code)
        {
            // Individual division endpoint might not use a wrapper
            return await GetAsync<Division>($"school/divisions/{code}");
        }
    }
}