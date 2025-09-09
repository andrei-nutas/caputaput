using System.Text.Json.Serialization;

namespace TAF_iSAMS.Test.API.Models.School
{
    /// <summary>
    /// Represents a year group in the school
    /// </summary>
    public class YearGroup
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("ncYear")]
        public int NcYear { get; set; }

        [JsonPropertyName("division")]
        public Division Division { get; set; }

        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; }
    }
}