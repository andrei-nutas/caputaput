using System.Text.Json.Serialization;

namespace TAF_iSAMS.Test.API.Models.School
{
    /// <summary>
    /// Represents the response from the year groups endpoint
    /// </summary>
    public class YearGroupResponse
    {
        [JsonPropertyName("yearGroups")]
        public List<YearGroup> YearGroups { get; set; }
    }
}