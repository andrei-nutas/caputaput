using System.Text.Json.Serialization;

namespace TAF_iSAMS.Test.API.Models.School
{
    /// <summary>
    /// Represents the response from the forms endpoint
    /// </summary>
    public class FormResponse
    {
        [JsonPropertyName("forms")]
        public List<Form> Forms { get; set; }
    }
}