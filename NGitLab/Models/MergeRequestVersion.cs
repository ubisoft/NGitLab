using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class MergeRequestVersion
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("head_commit_sha")]
    public string HeadCommitSha { get; set; }

    [JsonPropertyName("base_commit_sha")]
    public string BaseCommitSha { get; set; }

    [JsonPropertyName("start_commit_sha")]
    public string StartCommitSha { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("merge_request_id")]
    public long MergeRequestId { get; set; }

    [JsonPropertyName("state")]
    public string State { get; set; }

    [JsonPropertyName("real_size")]
    public string RealSize { get; set; }
}
