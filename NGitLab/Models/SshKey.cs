using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class SshKey
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("key")]
    public string Key { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreateAt { get; set; }
}
