using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class RunnerUpdate
{
    [Obsolete("Use Paused field instead")]
    [JsonPropertyName("active")]
    public bool? Active;

    [JsonPropertyName("description")]
    public string Description;

    [JsonPropertyName("paused")]
    public bool? Paused;

    [JsonPropertyName("locked")]
    public bool? Locked;

    [JsonPropertyName("run_untagged")]
    public bool? RunUntagged;

    [JsonPropertyName("tag_list")]
    public string[] TagList;
}
