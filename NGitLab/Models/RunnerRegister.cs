using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class RunnerRegister
{
    [JsonPropertyName("token")]
    public string Token { get; set; }

    [JsonPropertyName("paused")]
    public bool? Paused { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("locked")]
    public bool? Locked { get; set; }

    [JsonPropertyName("run_untagged")]
    public bool? RunUntagged { get; set; }

    [JsonPropertyName("tag_list")]
    public string[] TagList { get; set; }
}
