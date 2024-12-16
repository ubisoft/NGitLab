using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class Group
{
    public const string Url = "/groups";

    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("path")]
    public string Path { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("visibility")]
    public VisibilityLevel Visibility { get; set; }

    [JsonPropertyName("lfs_enabled")]
    public bool LfsEnabled { get; set; }

    [JsonPropertyName("avatar_url")]
    public string AvatarUrl { get; set; }

    [JsonPropertyName("request_access_enabled")]
    public bool RequestAccessEnabled { get; set; }

    [JsonPropertyName("full_name")]
    public string FullName { get; set; }

    [JsonPropertyName("full_path")]
    public string FullPath { get; set; }

    [JsonPropertyName("parent_id")]
    public long? ParentId { get; set; }

    [JsonPropertyName("runners_token")]
    public string RunnersToken { get; set; }

    [JsonPropertyName("projects")]
    public Project[] Projects { get; set; }

    [JsonPropertyName("shared_runners_minutes_limit")]
    public int? SharedRunnersMinutesLimit { get; set; }

    [JsonPropertyName("extra_shared_runners_minutes_limit")]
    public int? ExtraSharedRunnersMinutesLimit { get; set; }

    [JsonPropertyName("marked_for_deletion_on")]
    public string MarkedForDeletionOn { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }
}
