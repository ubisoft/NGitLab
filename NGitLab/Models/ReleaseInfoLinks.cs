using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class ReleaseInfoLinks
{
    [JsonPropertyName("self")]
    public string Self { get; set; }

    [JsonPropertyName("edit_url")]
    public string EditUrl { get; set; }
}
