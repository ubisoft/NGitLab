using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public sealed class SystemHookUpsert
{
    [Required]
    [JsonPropertyName("url")]
    public Uri Url { get; set; }

    [JsonPropertyName("token")]
    public string Token { get; set; }

    [JsonPropertyName("enable_ssl_verification")]
    public bool EnableSslVerification { get; set; }

    [JsonPropertyName("push_events")]
    public bool PushEvents { get; set; }

    [JsonPropertyName("tag_push_events")]
    public bool TagPushEvents { get; set; }

    [JsonPropertyName("merge_requests_events")]
    public bool MergeRequestsEvents { get; set; }

    [JsonPropertyName("repository_update_events")]
    public bool RepositoryUpdateEvents { get; set; }
}
