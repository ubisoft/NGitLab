using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class ReleaseInfo
{
    [JsonPropertyName("tag_name")]
    public string TagName { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("released_at")]
    public DateTime ReleasedAt { get; set; }

    [JsonPropertyName("author")]
    public Author Author { get; set; }

    [JsonPropertyName("commit")]
    public Commit Commit { get; set; }

    [JsonPropertyName("milestones")]
    public Milestone[] Milestones { get; set; }

    [JsonPropertyName("commit_path")]
    public string CommitPath { get; set; }

    [JsonPropertyName("tag_path")]
    public string TagPath { get; set; }

    [JsonPropertyName("assets")]
    public ReleaseAssetsInfo Assets { get; set; }

    [JsonPropertyName("evidences")]
    public ReleaseEvidence[] Evidences { get; set; }

    [JsonPropertyName("_links")]
    public ReleaseInfoLinks Links { get; set; }
}
