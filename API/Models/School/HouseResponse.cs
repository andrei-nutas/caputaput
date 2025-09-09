using System.Text.Json.Serialization;

namespace TAF_iSAMS.Test.API.Models.School
{
    /// <summary>
    /// Represents the response from the houses endpoint
    /// </summary>
    public class HouseResponse
    {
        [JsonPropertyName("houses")]
        public List<House> Houses { get; set; }
    }
}