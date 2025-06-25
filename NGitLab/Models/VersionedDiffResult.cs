using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class VersionedDiffResult
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("base_commit_sha")]
    public long BaseCommitSha { get; set; }

    [JsonPropertyName("commits")]
    public Commit[] Commits { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("diffs")]
    public Diff[] Diff { get; set; }

    [JsonPropertyName("head_commit_sha")]
    public string HeadCommitSha { get; set; }

    [JsonPropertyName("merge_request_id")]
    public long MergeRequestId { get; set; }

    [JsonPropertyName("patch_id_sha")]
    public string PatchIdSha { get; set; }

    [JsonPropertyName("real_size")]
    public string RealSize { get; set; }

    [JsonPropertyName("start_commit_sha")]
    public string StartCommitSha { get; set; }

    [JsonPropertyName("state")]
    public string State { get; set; }
}
