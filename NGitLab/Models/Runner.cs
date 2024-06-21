using System;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

[DebuggerDisplay("{Description}")]
public class Runner
{
    public const string Url = "/runners";

    [JsonPropertyName("id")]
    public int Id;

    [JsonPropertyName("name")]
    public string Name;

    [JsonPropertyName("paused")]
    public bool Paused;

    [Obsolete("Use Paused field instead")]
    [JsonPropertyName("active")]
    public bool Active;

    [JsonPropertyName("online")]
    public bool Online;

    [JsonPropertyName("status")]
    public string Status;

    [JsonPropertyName("description")]
    public string Description;

    [JsonPropertyName("is_shared")]
    public bool IsShared;

    [JsonPropertyName("contacted_at")]
    public DateTime ContactedAt;

    [JsonPropertyName("projects")]
    public Project[] Projects;

    [JsonPropertyName("groups")]
    public Group[] Groups;

    [JsonPropertyName("token")]
    public string Token;

    [JsonPropertyName("tag_list")]
    public string[] TagList;

    [JsonPropertyName("string")]
    public string Version;

    [JsonPropertyName("ip_address")]
    public string IpAddress;

    [JsonPropertyName("locked")]
    public bool Locked;

    [JsonPropertyName("run_untagged")]
    public bool RunUntagged;
}
