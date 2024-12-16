using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public sealed class SystemHook
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("url")]
    public Uri Url { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("push_events")]
    public bool PushEvents { get; set; }

    [JsonPropertyName("tag_push_events")]
    public bool TagPushEvents { get; set; }

    [JsonPropertyName("merge_requests_events")]
    public bool MergeRequestsEvents { get; set; }

    [JsonPropertyName("repository_update_events")]
    public bool RepositoryUpdateEvents { get; set; }

    [JsonPropertyName("enable_ssl_verification")]
    public bool EnableSslVerification { get; set; }
}
