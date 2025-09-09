using System.Text.Json.Serialization;

namespace TAF_iSAMS.Test.API.Models.School
{
    /// <summary>
    /// Represents a form in the school
    /// </summary>
    public class Form
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("emailAddress")]
        public string EmailAddress { get; set; }

        [JsonPropertyName("nationalCurriculumYear")]
        public int NationalCurriculumYear { get; set; }

        [JsonPropertyName("tutorId")]
        public int TutorId { get; set; }

        [JsonPropertyName("assistantTutor")]
        public int AssistantTutor { get; set; }

        [JsonPropertyName("secondAssistantTutor")]
        public int SecondAssistantTutor { get; set; }

        [JsonPropertyName("roomId")]
        public int RoomId { get; set; }

        // Adding a Name property for consistency with your tests
        // If this isn't in the API response, we can use the Code as Name
        public string Name => Code;
    }
}