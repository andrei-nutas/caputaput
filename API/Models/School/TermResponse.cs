using System.Text.Json.Serialization;

namespace TAF_iSAMS.Test.API.Models.School
{
    /// <summary>
    /// Represents the response from the terms endpoint
    /// </summary>
    public class TermResponse
    {
        [JsonPropertyName("terms")]
        public List<Term> Terms { get; set; }
    }
}