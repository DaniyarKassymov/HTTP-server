using System.Text.Json.Serialization;

namespace HTTPServer.Dtos;

public class TextModel
{
    [JsonPropertyName("text")]
    public string Text { get; set; }
}