using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class ProjectCreate
{
    [Required]
    [JsonPropertyName("name")]
    public string Name;

    [JsonPropertyName("namespace_id")]
    public long? NamespaceId { get; set; }

    /// <summary>
    /// The default branch name. Requires <see cref="InitializeWithReadme"/> to be true.
    /// </summary>
    [JsonPropertyName("default_branch")]
    public string DefaultBranch;

    [JsonPropertyName("initialize_with_readme")]
    public bool InitializeWithReadme;

    [JsonPropertyName("description")]
    public string Description;

    [JsonPropertyName("path")]
    public string Path;

    [JsonPropertyName("issues_enabled")]
    [Obsolete("Deprecated by GitLab. Use IssuesAccessLevel instead")]
    public bool IssuesEnabled;

    [JsonPropertyName("issues_access_level")]
    public string IssuesAccessLevel;

    [Obsolete("Deprecated by GitLab.")]
    [JsonIgnore]
    public bool WallEnabled;

    [JsonPropertyName("merge_pipelines_enabled")]
    public bool MergePipelinesEnabled;

    [JsonPropertyName("merge_requests_enabled")]
    [Obsolete("Deprecated by GitLab. Use MergeRequestsAccessLevel instead")]
    public bool MergeRequestsEnabled;

    [JsonPropertyName("merge_requests_access_level")]
    public string MergeRequestsAccessLevel;

    [JsonPropertyName("merge_trains_enabled")]
    public bool MergeTrainsEnabled;

    [JsonPropertyName("snippets_enabled")]
    [Obsolete("Deprecated by GitLab. Use SnippetsAccessLevel instead")]
    public bool SnippetsEnabled;

    [JsonPropertyName("snippets_access_level")]
    public string SnippetsAccessLevel;

    [JsonPropertyName("wiki_enabled")]
    [Obsolete("Deprecated by GitLab. Use WikiAccessLevel instead")]
    public bool WikiEnabled;

    [JsonPropertyName("wiki_access_level")]
    public string WikiAccessLevel;

    [JsonPropertyName("import_url")]
    public string ImportUrl = string.Empty;

    [JsonPropertyName("visibility")]
    public VisibilityLevel VisibilityLevel;

    [JsonPropertyName("tag_list")]
    [Obsolete("Deprecated by GitLab. Use Topics instead")]
    public List<string> Tags;

    [JsonPropertyName("topics")]
    public List<string> Topics { get; set; }

    /// <summary>
    /// The maximum amount of time, in seconds, that a job can run.
    /// </summary>
    [JsonPropertyName("build_timeout")]
    public int? BuildTimeout;

    [JsonPropertyName("squash_option")]
    public SquashOption? SquashOption { get; set; }
}
