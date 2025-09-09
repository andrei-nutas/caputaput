using TAF_iSAMS.Test.API.Auth;
using TAF_iSAMS.Test.API.Config;
using TAF_iSAMS.Test.API.Models.School;

namespace TAF_iSAMS.Test.API.Services.School
{
    /// <summary>
    /// Service for interacting with year group endpoints
    /// </summary>
    public class YearGroupService : BaseApiService
    {
        public YearGroupService(ApiConfig apiConfig, OAuthHandler oAuthHandler)
            : base(apiConfig, oAuthHandler)
        {
        }

        /// <summary>
        /// Gets all year groups
        /// </summary>
        /// <returns>A list of year groups</returns>
        public async Task<List<YearGroup>> GetAllYearGroupsAsync()
        {
            var response = await GetAsync<YearGroupResponse>("school/yeargroups");
            return response.YearGroups;
        }

        /// <summary>
        /// Gets a year group by NC year
        /// </summary>
        /// <param name="ncYear">The NC year</param>
        /// <returns>The year group</returns>
        public async Task<YearGroup> GetYearGroupByNcYearAsync(int ncYear)
        {
            // Individual year group endpoint might not use a wrapper
            return await GetAsync<YearGroup>($"school/yeargroups/{ncYear}");
        }
    }
}