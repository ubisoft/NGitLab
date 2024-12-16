using System;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

[DebuggerDisplay("{Description}")]
public class Runner
{
    public const string Url = "/runners";

    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("paused")]
    public bool Paused { get; set; }

    [JsonPropertyName("online")]
    public bool? Online { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("is_shared")]
    public bool IsShared { get; set; }

    [JsonPropertyName("contacted_at")]
    public DateTime ContactedAt { get; set; }

    [JsonPropertyName("projects")]
    public Project[] Projects { get; set; }

    [JsonPropertyName("groups")]
    public Group[] Groups { get; set; }

    [JsonPropertyName("token")]
    public string Token { get; set; }

    [JsonPropertyName("tag_list")]
    public string[] TagList { get; set; }

    [JsonPropertyName("string")]
    public string Version { get; set; }

    [JsonPropertyName("ip_address")]
    public string IpAddress { get; set; }

    [JsonPropertyName("locked")]
    public bool Locked { get; set; }

    [JsonPropertyName("run_untagged")]
    public bool RunUntagged { get; set; }
}
