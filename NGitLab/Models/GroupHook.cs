using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class GroupHook
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("url")]
    public Uri Url { get; set; }

    [JsonPropertyName("group_id")]
    public long GroupId { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("push_events")]
    public bool PushEvents { get; set; }

    [JsonPropertyName("merge_requests_events")]
    public bool MergeRequestsEvents { get; set; }

    [JsonPropertyName("issues_events")]
    public bool IssuesEvents { get; set; }

    [JsonPropertyName("tag_push_events")]
    public bool TagPushEvents { get; set; }

    [JsonPropertyName("note_events")]
    public bool NoteEvents { get; set; }

    [JsonPropertyName("job_events")]
    public bool JobEvents { get; set; }

    [JsonPropertyName("pipeline_events")]
    public bool PipelineEvents { get; set; }

    [JsonPropertyName("wiki_page_events")]
    public bool WikiPagesEvents { get; set; }

    [JsonPropertyName("enable_ssl_verification")]
    public bool EnableSslVerification { get; set; }

    [JsonPropertyName("token")]
    public string Token { get; set; }
}
