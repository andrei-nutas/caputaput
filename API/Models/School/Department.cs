using System.Text.Json.Serialization;

namespace TAF_iSAMS.Test.API.Models.School
{
    /// <summary>
    /// Represents a department in the school
    /// </summary>
    public class Department
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("departmentType")]
        public string DepartmentType { get; set; }

        [JsonPropertyName("subjects")]
        public List<Subject> Subjects { get; set; }
    }

    /// <summary>
    /// Represents a subject
    /// </summary>
    public class Subject
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }
    }
}