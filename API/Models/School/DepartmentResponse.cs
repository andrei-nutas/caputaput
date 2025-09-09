using System.Text.Json.Serialization;

namespace TAF_iSAMS.Test.API.Models.School
{
    /// <summary>
    /// Represents the response from the departments endpoint
    /// </summary>
    public class DepartmentResponse
    {
        [JsonPropertyName("departments")]
        public List<Department> Departments { get; set; }
    }
}