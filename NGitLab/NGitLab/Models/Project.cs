using System;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Project
    {
        public const string Url = "/projects";

        [DataMember(Name = "id")]
        public int Id;

        [DataMember(Name = "avatar_url")]
        public string AvatarUrl;

        [DataMember(Name = "name")]
        public string Name;

        [DataMember(Name = "name_with_namespace")]
        public string NameWithNamespace;

        [DataMember(Name = "open_issues_count")]
        public int OpenIssuesCount;

        [DataMember(Name = "jobs_enabled")]
        [Obsolete("Deprecated by Gitlab. Use BuildsAccessLevel instead")]
        public bool JobEnabled;

        [DataMember(Name = "builds_access_level")]
        public string BuildsAccessLevel;

        [DataMember(Name = "snippets_enabled")]
        [Obsolete("Deprecated by Gitlab. Use SnippetsAccessLevel instead")]
        public bool SnippetsEnabled;

        [DataMember(Name = "snippets_access_level")]
        public string SnippetsAccessLevel;

        [DataMember(Name = "resolve_outdated_diff_discussions")]
        public bool ResolveOutdatedDiffDiscussions;

        [DataMember(Name = "container_registry_enabled")]
        public bool ContainerRegistryEnabled;

        [DataMember(Name = "description")]
        public string Description;

        [DataMember(Name = "default_branch")]
        public string DefaultBranch;

        [DataMember(Name = "owner")]
        public User Owner;

        [DataMember(Name = "public")]
        public bool Public;

        [DataMember(Name = "path")]
        public string Path;

        [DataMember(Name = "path_with_namespace")]
        public string PathWithNamespace;

        [DataMember(Name = "issues_enabled")]
        [Obsolete("Deprecated by Gitlab. Use IssuesAccessLevel instead")]
        public bool IssuesEnabled;

        [DataMember(Name = "issues_access_level")]
        public string IssuesAccessLevel;

        [DataMember(Name = "merge_requests_enabled")]
        [Obsolete("Deprecated by Gitlab. Use MergeRequestsAccessLevel instead")]
        public bool MergeRequestsEnabled;

        [DataMember(Name = "merge_requests_access_level")]
        public string MergeRequestsAccessLevel;

        [DataMember(Name = "wall_enabled")]
        public bool WallEnabled;

        [DataMember(Name = "wiki_enabled")]
        [Obsolete("Deprecated by Gitlab. Use WikiAccessLevel instead")]
        public bool WikiEnabled;

        [DataMember(Name = "wiki_access_level")]
        public string WikiAccessLevel;

        [DataMember(Name = "created_at")]
        public DateTime CreatedAt;

        [DataMember(Name = "last_activity_at")]
        public DateTime LastActivityAt;

        [DataMember(Name = "creator_id")] 
        public int CreatorId;

        [DataMember(Name = "ssh_url_to_repo")]
        public string SshUrl;

        [DataMember(Name = "http_url_to_repo")]
        public string HttpUrl;

        [DataMember(Name = "web_url")]
        public string WebUrl;

        [DataMember(Name = "namespace")]
        public Namespace Namespace;

        [DataMember(Name = "import_status")] 
        public string ImportStatus;

        [DataMember(Name = "import_error")]
        public string ImportError;

        [DataMember(Name = "archived")]
        public bool Archived;

        [DataMember(Name = "shared_runners_enabled")]
        public bool SharedRunnersEnabled;

        [DataMember(Name = "tag_list")]
        public string[] TagList;

        [DataMember(Name = "star_count")]
        public int StarCount;

        [DataMember(Name = "forks_count")]
        public int ForksCount;

        [DataMember(Name = "runners_token")]
        public string RunnersToken;

        [DataMember(Name = "public_jobs")]
        public bool PublicJobs;

        [DataMember(Name = "forked_from_project")]
        public Project ForkedFromProject;

        [DataMember(Name = "repository_storage")]
        public string RepositoryStorage;

        [DataMember(Name = "only_allow_merge_if_pipeline_succeeds")]
        public bool OnlyAllowMergeIfPipelineSucceeds;

        [DataMember(Name = "only_allow_merge_if_all_discussions_are_resolved")]
        public bool OnlyAllowMergeIfDiscussionsAreResolved;

        [DataMember(Name = "printing_merge_requests_link_enabled")]
        public bool PrintingMergeRequestsLinkEnabled;

        [DataMember(Name = "request_access_enabled")]
        public bool RequestAccessEnabled;

        [DataMember(Name = "approvals_before_merge")]
        public int ApprovalsBeforeMerge;

        [DataMember(Name = "statistics")]
        public ProjectStatistics Statistics;

        [DataMember(Name = "_links")]
        public ProjectLinks Links;

        [DataMember(Name = "visibility")]
        public VisibilityLevel VisibilityLevel;
    }
}