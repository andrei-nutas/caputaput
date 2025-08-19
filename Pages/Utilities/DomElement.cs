
using System.Text.Json;
using System.Text.Json.Serialization;

public class DomElement
{
    [JsonPropertyName("tag")]
    public string Tag { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("classList")]
    public List<string> ClassList { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }
}
