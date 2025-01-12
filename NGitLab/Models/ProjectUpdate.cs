using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public sealed class ProjectUpdate
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("path")]
    public string Path { get; set; }

    [JsonPropertyName("default_branch")]
    public string DefaultBranch { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("issues_enabled")]
    [Obsolete("Deprecated by GitLab. Use IssuesAccessLevel instead")]
    public bool? IssuesEnabled { get; set; }

    [JsonIgnore]
    [Obsolete("Use IssuesAccessLevel instead")]
    public string IssuesAccessLeve { get => IssuesAccessLevel; set => IssuesAccessLevel = value; }

    [JsonPropertyName("issues_access_level")]
    public string IssuesAccessLevel { get; set; }

    [JsonPropertyName("merge_pipelines_enabled")]
    public bool MergePipelinesEnabled { get; set; }

    [JsonPropertyName("merge_requests_enabled")]
    [Obsolete("Deprecated by GitLab. Use MergeRequestsAccessLevel instead")]
    public bool? MergeRequestsEnabled { get; set; }

    [JsonPropertyName("merge_requests_access_level")]
    public string MergeRequestsAccessLevel { get; set; }

    [JsonPropertyName("merge_trains_enabled")]
    public bool MergeTrainsEnabled { get; set; }

    [JsonPropertyName("jobs_enabled")]
    [Obsolete("Deprecated by GitLab. Use BuildsAccessLevel instead")]
    public bool? JobsEnabled { get; set; }

    [JsonPropertyName("builds_access_level")]
    public string BuildsAccessLevel { get; set; }

    [JsonPropertyName("wiki_enabled")]
    [Obsolete("Deprecated by GitLab. Use WikiAccessLevel instead")]
    public bool? WikiEnabled { get; set; }

    [JsonPropertyName("wiki_access_level")]
    public string WikiAccessLevel { get; set; }

    [JsonPropertyName("snippets_enabled")]
    [Obsolete("Deprecated by GitLab. Use SnippetsAccessLevel instead")]
    public bool? SnippetsEnabled { get; set; }

    [JsonPropertyName("snippets_access_level")]
    public string SnippetsAccessLevel { get; set; }

    [JsonPropertyName("resolve_outdated_diff_discussions")]
    public bool? ResolveOutdatedDiffDiscussions { get; set; }

    [JsonPropertyName("container_registry_enabled")]
    public bool? ContainerRegistryEnabled { get; set; }

    [JsonPropertyName("shared_runners_enabled")]
    public bool? SharedRunnersEnabled { get; set; }

    [JsonPropertyName("group_runners_enabled")]
    public bool? GroupRunnersEnabled { get; set; }

    [JsonPropertyName("visibility")]
    public VisibilityLevel? Visibility { get; set; }

    [JsonPropertyName("public_builds")]
    public bool? PublicBuilds { get; set; }

    [JsonPropertyName("only_allow_merge_if_pipeline_succeeds")]
    public bool? OnlyAllowMergeIfPipelineSucceeds { get; set; }

    [JsonPropertyName("only_allow_merge_if_all_discussions_are_resolved")]
    public bool? OnlyAllowMergeIfAllDiscussionsAreResolved { get; set; }

    [JsonPropertyName("remove_source_branch_after_merge")]
    public bool? RemoveSourceBranchAfterMerge { get; set; }

    [JsonPropertyName("lfs_enabled")]
    public bool? LfsEnabled { get; set; }

    [JsonPropertyName("request_access_enabled")]
    public bool? RequestAccessEnabled { get; set; }

    [JsonPropertyName("repository_access_level")]
    public string RepositoryAccessLevel { get; set; }

    [JsonPropertyName("packages_enabled")]
    public bool? PackagesEnabled { get; set; }

    [JsonPropertyName("build_timeout")]
    public int? BuildTimeout { get; set; }

    [JsonPropertyName("tag_list")]
    [Obsolete("Deprecated by GitLab. Use Topics instead")]
    public string[] TagList { get; set; }

    [JsonPropertyName("topics")]
    public List<string> Topics { get; set; }

    [JsonPropertyName("analytics_access_level")]
    public RepositoryAccessLevel? AnalyticsAccessLevel { get; set; }

    [JsonPropertyName("environments_access_level")]
    public RepositoryAccessLevel? EnvironmentsAccessLevel { get; set; }

    [JsonPropertyName("feature_flags_access_level")]
    public RepositoryAccessLevel? FeatureFlagsAccessLevel { get; set; }

    [JsonPropertyName("forking_access_level")]
    public RepositoryAccessLevel? ForkingAccessLevel { get; set; }

    [JsonPropertyName("infrastructure_access_level")]
    public RepositoryAccessLevel? InfrastructureAccessLevel { get; set; }

    [JsonPropertyName("model_experiments_access_level")]
    public RepositoryAccessLevel? ModelExperimentsAccessLevel { get; set; }

    [JsonPropertyName("model_registry_access_level")]
    public RepositoryAccessLevel? ModelRegistryAccessLevel { get; set; }

    [JsonPropertyName("monitor_access_level")]
    public RepositoryAccessLevel? MonitorAccessLevel { get; set; }

    [JsonPropertyName("pages_access_level")]
    public RepositoryAccessLevel? PagesAccessLevel { get; set; }

    [JsonPropertyName("requirements_access_level")]
    public RepositoryAccessLevel? RequirementsAccessLevel { get; set; }

    [JsonPropertyName("operations_access_level")]
    public RepositoryAccessLevel? OperationsAccessLevel { get; set; }

    [JsonPropertyName("security_and_compliance_access_level")]
    public RepositoryAccessLevel? SecurityAndComplianceAccessLevel { get; set; }

    [JsonPropertyName("service_desk_enabled")]
    public bool? ServiceDeskEnabled { get; set; }
}
