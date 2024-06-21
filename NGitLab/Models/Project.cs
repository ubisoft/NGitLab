using System;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

[DebuggerDisplay("{" + nameof(PathWithNamespace) + "}")]
public class Project
{
    public const string Url = "/projects";

    [JsonPropertyName("id")]
    public int Id;

    [JsonPropertyName("avatar_url")]
    public string AvatarUrl;

    [JsonPropertyName("name")]
    public string Name;

    [JsonPropertyName("name_with_namespace")]
    public string NameWithNamespace;

    [JsonPropertyName("open_issues_count")]
    public int OpenIssuesCount;

    [JsonPropertyName("jobs_enabled")]
    [Obsolete("Deprecated by GitLab. Use BuildsAccessLevel instead")]
    public bool JobEnabled;

    [JsonPropertyName("builds_access_level")]
    public string BuildsAccessLevel;

    [JsonPropertyName("snippets_enabled")]
    [Obsolete("Deprecated by GitLab. Use SnippetsAccessLevel instead")]
    public bool SnippetsEnabled;

    [JsonPropertyName("snippets_access_level")]
    public string SnippetsAccessLevel;

    [JsonPropertyName("resolve_outdated_diff_discussions")]
    public bool ResolveOutdatedDiffDiscussions;

    [JsonPropertyName("container_registry_enabled")]
    public bool ContainerRegistryEnabled;

    [JsonPropertyName("description")]
    public string Description;

    [JsonPropertyName("default_branch")]
    public string DefaultBranch;

    [JsonPropertyName("owner")]
    public User Owner;

    [JsonPropertyName("public")]
    public bool Public;

    [JsonPropertyName("path")]
    public string Path;

    [JsonPropertyName("path_with_namespace")]
    public string PathWithNamespace;

    [JsonPropertyName("issues_enabled")]
    [Obsolete("Deprecated by GitLab. Use IssuesAccessLevel instead")]
    public bool IssuesEnabled;

    [JsonPropertyName("issues_access_level")]
    public string IssuesAccessLevel;

    [JsonPropertyName("merge_pipelines_enabled")]
    public bool MergePipelinesEnabled;

    [JsonPropertyName("merge_requests_enabled")]
    [Obsolete("Deprecated by GitLab. Use MergeRequestsAccessLevel instead")]
    public bool MergeRequestsEnabled;

    [JsonPropertyName("merge_requests_access_level")]
    public string MergeRequestsAccessLevel;

    [JsonPropertyName("merge_trains_enabled")]
    public bool MergeTrainsEnabled;

    [JsonPropertyName("repository_access_level")]
    public RepositoryAccessLevel RepositoryAccessLevel;

    [JsonPropertyName("merge_method")]
    public string MergeMethod;

    [JsonPropertyName("wall_enabled")]
    public bool WallEnabled;

    [JsonPropertyName("wiki_enabled")]
    [Obsolete("Deprecated by GitLab. Use WikiAccessLevel instead")]
    public bool WikiEnabled;

    [JsonPropertyName("wiki_access_level")]
    public string WikiAccessLevel;

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt;

    [JsonPropertyName("last_activity_at")]
    public DateTime LastActivityAt;

    [JsonPropertyName("creator_id")]
    public int CreatorId;

    [JsonPropertyName("ssh_url_to_repo")]
    public string SshUrl;

    [JsonPropertyName("http_url_to_repo")]
    public string HttpUrl;

    [JsonPropertyName("web_url")]
    public string WebUrl;

    [JsonPropertyName("namespace")]
    public Namespace Namespace;

    [JsonPropertyName("import_status")]
    public string ImportStatus;

    [JsonPropertyName("import_error")]
    public string ImportError;

    [JsonPropertyName("archived")]
    public bool Archived;

    [JsonPropertyName("shared_runners_enabled")]
    public bool SharedRunnersEnabled;

    [JsonPropertyName("group_runners_enabled")]
    public bool GroupRunnersEnabled;

    [JsonPropertyName("tag_list")]
    [Obsolete("Deprecated by GitLab. Use Topics instead")]
    public string[] TagList;

    [JsonPropertyName("topics")]
    public string[] Topics { get; set; }

    [JsonPropertyName("star_count")]
    public int StarCount;

    [JsonPropertyName("forks_count")]
    public int ForksCount;

    [JsonPropertyName("forking_access_level")]
    public RepositoryAccessLevel ForkingAccessLevel;

    [JsonPropertyName("runners_token")]
    public string RunnersToken;

    [JsonPropertyName("public_jobs")]
    public bool PublicJobs;

    [JsonPropertyName("forked_from_project")]
    public Project ForkedFromProject;

    [JsonPropertyName("repository_storage")]
    public string RepositoryStorage;

    [JsonPropertyName("only_allow_merge_if_pipeline_succeeds")]
    public bool OnlyAllowMergeIfPipelineSucceeds;

    [JsonPropertyName("only_allow_merge_if_all_discussions_are_resolved")]
    public bool OnlyAllowMergeIfDiscussionsAreResolved;

    [JsonPropertyName("printing_merge_requests_link_enabled")]
    public bool PrintingMergeRequestsLinkEnabled;

    [JsonPropertyName("request_access_enabled")]
    public bool RequestAccessEnabled;

    [JsonPropertyName("approvals_before_merge")]
    public int ApprovalsBeforeMerge;

    [JsonPropertyName("statistics")]
    public ProjectStatistics Statistics;

    [JsonPropertyName("_links")]
    public ProjectLinks Links;

    [JsonPropertyName("visibility")]
    public VisibilityLevel VisibilityLevel;

    /// <summary>
    /// The maximum amount of time, in seconds, that a job can run.
    /// </summary>
    [JsonPropertyName("build_timeout")]
    public int? BuildTimeout;

    [JsonPropertyName("lfs_enabled")]
    public bool LfsEnabled;

    [JsonPropertyName("empty_repo")]
    public bool EmptyRepo;

    [JsonPropertyName("mirror")]
    public bool Mirror;

    [JsonPropertyName("mirror_user_id")]
    public int MirrorUserId;

    [JsonPropertyName("mirror_trigger_builds")]
    public bool MirrorTriggerBuilds;

    [JsonPropertyName("only_mirror_protected_branches")]
    public bool OnlyMirrorProtectedBranch;

    [JsonPropertyName("mirror_overwrites_diverged_branches")]
    public bool MirrorOverwritesDivergedBranches;

    [JsonPropertyName("squash_option")]
    public DynamicEnum<SquashOption> SquashOption;

    [JsonPropertyName("permissions")]
    public ProjectPermissions Permissions;

    [JsonPropertyName("releases_access_level")]
    public ReleasesAccessLevel ReleasesAccessLevel { get; set; }
}
