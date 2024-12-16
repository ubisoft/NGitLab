using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class SshKey
{
    [JsonPropertyName("id")]
    public long Id;

    [JsonPropertyName("title")]
    public string Title;

    [JsonPropertyName("key")]
    public string Key;

    [JsonPropertyName("created_at")]
    public DateTime CreateAt;
}
