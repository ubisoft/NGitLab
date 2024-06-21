using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class Group
{
    public const string Url = "/groups";

    [JsonPropertyName("id")]
    public int Id;

    [JsonPropertyName("name")]
    public string Name;

    [JsonPropertyName("path")]
    public string Path;

    [JsonPropertyName("description")]
    public string Description;

    [JsonPropertyName("visibility")]
    public VisibilityLevel Visibility;

    [JsonPropertyName("lfs_enabled")]
    public bool LfsEnabled;

    [JsonPropertyName("avatar_url")]
    public string AvatarUrl;

    [JsonPropertyName("request_access_enabled")]
    public bool RequestAccessEnabled;

    [JsonPropertyName("full_name")]
    public string FullName;

    [JsonPropertyName("full_path")]
    public string FullPath;

    [JsonPropertyName("parent_id")]
    public int? ParentId;

    [JsonPropertyName("runners_token")]
    public string RunnersToken;

    [JsonPropertyName("projects")]
    public Project[] Projects;

    [JsonPropertyName("shared_runners_minutes_limit")]
    public int? SharedRunnersMinutesLimit;

    [JsonPropertyName("extra_shared_runners_minutes_limit")]
    public int? ExtraSharedRunnersMinutesLimit;

    [JsonPropertyName("marked_for_deletion_on")]
    public string MarkedForDeletionOn;

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt;
}
