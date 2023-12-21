using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class ReleaseAssetSource
{
    [JsonPropertyName("format")]
    public string Format { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }
}
