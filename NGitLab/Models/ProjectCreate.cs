using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class ProjectCreate
{
    [Required]
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("namespace_id")]
    public long? NamespaceId { get; set; }

    /// <summary>
    /// The default branch name. Requires <see cref="InitializeWithReadme"/> to be true.
    /// </summary>
    [JsonPropertyName("default_branch")]
    public string DefaultBranch { get; set; }

    [JsonPropertyName("initialize_with_readme")]
    public bool InitializeWithReadme { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("path")]
    public string Path { get; set; }

    [JsonPropertyName("issues_enabled")]
    [Obsolete("Deprecated by GitLab. Use IssuesAccessLevel instead")]
    public bool IssuesEnabled { get; set; }

    [JsonPropertyName("issues_access_level")]
    public string IssuesAccessLevel { get; set; }

    [Obsolete("Deprecated by GitLab.")]
    [JsonIgnore]
    public bool WallEnabled { get; set; }

    [JsonPropertyName("merge_pipelines_enabled")]
    public bool MergePipelinesEnabled { get; set; }

    [JsonPropertyName("merge_requests_enabled")]
    [Obsolete("Deprecated by GitLab. Use MergeRequestsAccessLevel instead")]
    public bool MergeRequestsEnabled { get; set; }

    [JsonPropertyName("merge_requests_access_level")]
    public string MergeRequestsAccessLevel { get; set; }

    [JsonPropertyName("merge_trains_enabled")]
    public bool MergeTrainsEnabled { get; set; }

    [JsonPropertyName("snippets_enabled")]
    [Obsolete("Deprecated by GitLab. Use SnippetsAccessLevel instead")]
    public bool SnippetsEnabled { get; set; }

    [JsonPropertyName("snippets_access_level")]
    public string SnippetsAccessLevel { get; set; }

    [JsonPropertyName("wiki_enabled")]
    [Obsolete("Deprecated by GitLab. Use WikiAccessLevel instead")]
    public bool WikiEnabled { get; set; }

    [JsonPropertyName("wiki_access_level")]
    public string WikiAccessLevel { get; set; }

    [JsonPropertyName("import_url")]
    public string ImportUrl { get; set; } = string.Empty;

    [JsonPropertyName("visibility")]
    public VisibilityLevel VisibilityLevel { get; set; }

    [JsonPropertyName("tag_list")]
    [Obsolete("Deprecated by GitLab. Use Topics instead")]
    public List<string> Tags { get; set; }

    [JsonPropertyName("topics")]
    public List<string> Topics { get; set; }

    /// <summary>
    /// The maximum amount of time, in seconds, that a job can run.
    /// </summary>
    [JsonPropertyName("build_timeout")]
    public int? BuildTimeout { get; set; }

    [JsonPropertyName("squash_option")]
    public SquashOption? SquashOption { get; set; }
}
