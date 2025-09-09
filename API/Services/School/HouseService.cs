using System.Text.Json.Serialization;
using TAF_iSAMS.Test.API.Auth;
using TAF_iSAMS.Test.API.Config;
using TAF_iSAMS.Test.API.Models.School;

namespace TAF_iSAMS.Test.API.Services.School
{
    /// <summary>
    /// Service for interacting with house endpoints
    /// </summary>
    public class HouseService : BaseApiService
    {
        public HouseService(ApiConfig apiConfig, OAuthHandler oAuthHandler)
            : base(apiConfig, oAuthHandler)
        {
        }

        /// <summary>
        /// Gets all houses
        /// </summary>
        /// <returns>A list of houses</returns>
        public async Task<List<House>> GetAllHousesAsync()
        {
            // Get the wrapped response and return just the houses
            var response = await GetAsync<HouseResponse>("school/houses");
            return response.Houses;
        }

        /// <summary>
        /// Gets houses by type
        /// </summary>
        /// <param name="houseType">The house type (e.g., "academic", "boarding")</param>
        /// <returns>A list of houses</returns>
        public async Task<List<House>> GetHousesByTypeAsync(string houseType)
        {
            var response = await GetAsync<HouseResponse>($"school/houses?houseType={houseType}");
            return response.Houses;
        }

        /// <summary>
        /// Gets a house by ID
        /// </summary>
        /// <param name="houseId">The house ID</param>
        /// <returns>The house</returns>
        public async Task<House> GetHouseByIdAsync(int houseId)
        {
            // The single house endpoint might also return a wrapper
            try
            {
                // First try direct conversion
                return await GetAsync<House>($"school/houses/{houseId}");
            }
            catch (System.Text.Json.JsonException)
            {
                // If direct conversion fails, try with a wrapper
                var response = await GetAsync<SingleHouseResponse>($"school/houses/{houseId}");
                return response.House;
            }
        }
    }

    /// <summary>
    /// Represents the response from a single house endpoint
    /// </summary>
    public class SingleHouseResponse
    {
        [JsonPropertyName("house")]
        public House House { get; set; }
    }
}