using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class RunnerUpdate
{
    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("paused")]
    public bool? Paused { get; set; }

    [JsonPropertyName("locked")]
    public bool? Locked { get; set; }

    [JsonPropertyName("run_untagged")]
    public bool? RunUntagged { get; set; }

    [JsonPropertyName("tag_list")]
    public string[] TagList { get; set; }
}
