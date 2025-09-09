using System.Text.Json.Serialization;

namespace TAF_iSAMS.Test.API.Models.School
{
    /// <summary>
    /// Represents the response from the divisions endpoint
    /// </summary>
    public class DivisionResponse
    {
        [JsonPropertyName("divisions")]
        public List<Division> Divisions { get; set; }
    }
}