using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class RunnerUpdate
{
    [JsonPropertyName("active")]
    public bool? Active;

    [JsonPropertyName("description")]
    public string Description;

    [JsonPropertyName("locked")]
    public bool? Locked;

    [JsonPropertyName("run_untagged")]
    public bool? RunUntagged;

    [JsonPropertyName("tag_list")]
    public string[] TagList;
}
