using System.Text.Json.Serialization;

namespace TAF_iSAMS.Test.API.Models.School
{
    /// <summary>
    /// Represents a term in the school calendar
    /// </summary>
    public class Term
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("academicYear")]
        public string AcademicYear { get; set; }

        [JsonPropertyName("startDate")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("endDate")]
        public DateTime EndDate { get; set; }

        [JsonPropertyName("isCurrentTerm")]
        public bool IsCurrentTerm { get; set; }
    }
}