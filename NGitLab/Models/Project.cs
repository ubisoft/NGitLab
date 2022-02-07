using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    [DebuggerDisplay("{" + nameof(PathWithNamespace) + "}")]
    public class Project
    {
        public const string Url = "/projects";

        [DataMember(Name = "id")]
        [JsonPropertyName("id")]
        public int Id;

        [DataMember(Name = "avatar_url")]
        [JsonPropertyName("avatar_url")]
        public string AvatarUrl;

        [DataMember(Name = "name")]
        [JsonPropertyName("name")]
        public string Name;

        [DataMember(Name = "name_with_namespace")]
        [JsonPropertyName("name_with_namespace")]
        public string NameWithNamespace;

        [DataMember(Name = "open_issues_count")]
        [JsonPropertyName("open_issues_count")]
        public int OpenIssuesCount;

        [DataMember(Name = "jobs_enabled")]
        [JsonPropertyName("jobs_enabled")]
        [Obsolete("Deprecated by GitLab. Use BuildsAccessLevel instead")]
        public bool JobEnabled;

        [DataMember(Name = "builds_access_level")]
        [JsonPropertyName("builds_access_level")]
        public string BuildsAccessLevel;

        [DataMember(Name = "snippets_enabled")]
        [JsonPropertyName("snippets_enabled")]
        [Obsolete("Deprecated by GitLab. Use SnippetsAccessLevel instead")]
        public bool SnippetsEnabled;

        [DataMember(Name = "snippets_access_level")]
        [JsonPropertyName("snippets_access_level")]
        public string SnippetsAccessLevel;

        [DataMember(Name = "resolve_outdated_diff_discussions")]
        [JsonPropertyName("resolve_outdated_diff_discussions")]
        public bool ResolveOutdatedDiffDiscussions;

        [DataMember(Name = "container_registry_enabled")]
        [JsonPropertyName("container_registry_enabled")]
        public bool ContainerRegistryEnabled;

        [DataMember(Name = "description")]
        [JsonPropertyName("description")]
        public string Description;

        [DataMember(Name = "default_branch")]
        [JsonPropertyName("default_branch")]
        public string DefaultBranch;

        [DataMember(Name = "owner")]
        [JsonPropertyName("owner")]
        public User Owner;

        [DataMember(Name = "public")]
        [JsonPropertyName("public")]
        public bool Public;

        [DataMember(Name = "path")]
        [JsonPropertyName("path")]
        public string Path;

        [DataMember(Name = "path_with_namespace")]
        [JsonPropertyName("path_with_namespace")]
        public string PathWithNamespace;

        [DataMember(Name = "issues_enabled")]
        [JsonPropertyName("issues_enabled")]
        [Obsolete("Deprecated by GitLab. Use IssuesAccessLevel instead")]
        public bool IssuesEnabled;

        [DataMember(Name = "issues_access_level")]
        [JsonPropertyName("issues_access_level")]
        public string IssuesAccessLevel;

        [DataMember(Name = "merge_requests_enabled")]
        [JsonPropertyName("merge_requests_enabled")]
        [Obsolete("Deprecated by GitLab. Use MergeRequestsAccessLevel instead")]
        public bool MergeRequestsEnabled;

        [DataMember(Name = "merge_requests_access_level")]
        [JsonPropertyName("merge_requests_access_level")]
        public string MergeRequestsAccessLevel;

        [DataMember(Name = "repository_access_level")]
        [JsonPropertyName("repository_access_level")]
        public RepositoryAccessLevel RepositoryAccessLevel;

        [DataMember(Name = "merge_method")]
        [JsonPropertyName("merge_method")]
        public string MergeMethod;

        [DataMember(Name = "wall_enabled")]
        [JsonPropertyName("wall_enabled")]
        public bool WallEnabled;

        [DataMember(Name = "wiki_enabled")]
        [JsonPropertyName("wiki_enabled")]
        [Obsolete("Deprecated by GitLab. Use WikiAccessLevel instead")]
        public bool WikiEnabled;

        [DataMember(Name = "wiki_access_level")]
        [JsonPropertyName("wiki_access_level")]
        public string WikiAccessLevel;

        [DataMember(Name = "created_at")]
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt;

        [DataMember(Name = "last_activity_at")]
        [JsonPropertyName("last_activity_at")]
        public DateTime LastActivityAt;

        [DataMember(Name = "creator_id")]
        [JsonPropertyName("creator_id")]
        public int CreatorId;

        [DataMember(Name = "ssh_url_to_repo")]
        [JsonPropertyName("ssh_url_to_repo")]
        public string SshUrl;

        [DataMember(Name = "http_url_to_repo")]
        [JsonPropertyName("http_url_to_repo")]
        public string HttpUrl;

        [DataMember(Name = "web_url")]
        [JsonPropertyName("web_url")]
        public string WebUrl;

        [DataMember(Name = "namespace")]
        [JsonPropertyName("namespace")]
        public Namespace Namespace;

        [DataMember(Name = "import_status")]
        [JsonPropertyName("import_status")]
        public string ImportStatus;

        [DataMember(Name = "import_error")]
        [JsonPropertyName("import_error")]
        public string ImportError;

        [DataMember(Name = "archived")]
        [JsonPropertyName("archived")]
        public bool Archived;

        [DataMember(Name = "shared_runners_enabled")]
        [JsonPropertyName("shared_runners_enabled")]
        public bool SharedRunnersEnabled;

        [DataMember(Name = "tag_list")]
        [JsonPropertyName("tag_list")]
        public string[] TagList;

        [DataMember(Name = "star_count")]
        [JsonPropertyName("star_count")]
        public int StarCount;

        [DataMember(Name = "forks_count")]
        [JsonPropertyName("forks_count")]
        public int ForksCount;

        [DataMember(Name = "forking_access_level")]
        [JsonPropertyName("forking_access_level")]
        public RepositoryAccessLevel ForkingAccessLevel;

        [DataMember(Name = "runners_token")]
        [JsonPropertyName("runners_token")]
        public string RunnersToken;

        [DataMember(Name = "public_jobs")]
        [JsonPropertyName("public_jobs")]
        public bool PublicJobs;

        [DataMember(Name = "forked_from_project")]
        [JsonPropertyName("forked_from_project")]
        public Project ForkedFromProject;

        [DataMember(Name = "repository_storage")]
        [JsonPropertyName("repository_storage")]
        public string RepositoryStorage;

        [DataMember(Name = "only_allow_merge_if_pipeline_succeeds")]
        [JsonPropertyName("only_allow_merge_if_pipeline_succeeds")]
        public bool OnlyAllowMergeIfPipelineSucceeds;

        [DataMember(Name = "only_allow_merge_if_all_discussions_are_resolved")]
        [JsonPropertyName("only_allow_merge_if_all_discussions_are_resolved")]
        public bool OnlyAllowMergeIfDiscussionsAreResolved;

        [DataMember(Name = "printing_merge_requests_link_enabled")]
        [JsonPropertyName("printing_merge_requests_link_enabled")]
        public bool PrintingMergeRequestsLinkEnabled;

        [DataMember(Name = "request_access_enabled")]
        [JsonPropertyName("request_access_enabled")]
        public bool RequestAccessEnabled;

        [DataMember(Name = "approvals_before_merge")]
        [JsonPropertyName("approvals_before_merge")]
        public int ApprovalsBeforeMerge;

        [DataMember(Name = "statistics")]
        [JsonPropertyName("statistics")]
        public ProjectStatistics Statistics;

        [DataMember(Name = "_links")]
        [JsonPropertyName("_links")]
        public ProjectLinks Links;

        [DataMember(Name = "visibility")]
        [JsonPropertyName("visibility")]
        public VisibilityLevel VisibilityLevel;

        [DataMember(Name = "build_timeout")]
        [JsonPropertyName("build_timeout")]
        public int? BuildTimeout;

        [DataMember(Name = "lfs_enabled")]
        [JsonPropertyName("lfs_enabled")]
        public bool LfsEnabled;

        [DataMember(Name = "empty_repo")]
        [JsonPropertyName("empty_repo")]
        public bool EmptyRepo;

        [DataMember(Name = "mirror")]
        [JsonPropertyName("mirror")]
        public bool Mirror;

        [DataMember(Name = "mirror_user_id")]
        [JsonPropertyName("mirror_user_id")]
        public int MirrorUserId;

        [DataMember(Name = "mirror_trigger_builds")]
        [JsonPropertyName("mirror_trigger_builds")]
        public bool MirrorTriggerBuilds;

        [DataMember(Name = "only_mirror_protected_branches")]
        [JsonPropertyName("only_mirror_protected_branches")]
        public bool OnlyMirrorProtectedBranch;

        [DataMember(Name = "mirror_overwrites_diverged_branches")]
        [JsonPropertyName("mirror_overwrites_diverged_branches")]
        public bool MirrorOverwritesDivergedBranches;

        [DataMember(Name = "squash_option")]
        [JsonPropertyName("squash_option")]
        public DynamicEnum<SquashOption> SquashOption;
    }
}
