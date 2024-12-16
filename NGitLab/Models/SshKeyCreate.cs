using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class SshKeyCreate
{
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("key")]
    public string Key { get; set; }
}
