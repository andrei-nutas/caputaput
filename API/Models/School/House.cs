using System.Text.Json.Serialization;

namespace TAF_iSAMS.Test.API.Models.School
{
    /// <summary>
    /// Represents a house in the school
    /// </summary>
    public class House
    {
        [JsonPropertyName("id")]
        public int Id { get; set; } // Changed from string to int

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("type")]
        public string HouseType { get; set; }

        [JsonPropertyName("gender")]
        public string Gender { get; set; }

        [JsonPropertyName("addressLine")]
        public string AddressLine { get; set; }

        [JsonPropertyName("town")]
        public string Town { get; set; }

        [JsonPropertyName("county")]
        public string County { get; set; }

        [JsonPropertyName("postcode")]
        public string Postcode { get; set; }

        [JsonPropertyName("telephone")]
        public string Telephone { get; set; }

        [JsonPropertyName("fax")]
        public string Fax { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("website")]
        public string Website { get; set; }

        [JsonPropertyName("houseMasterEmployeeId")]
        public int? HouseMasterEmployeeId { get; set; }

        [JsonPropertyName("assistantHouseMasterEmployeeId")]
        public int? AssistantHouseMasterEmployeeId { get; set; }
    }
}