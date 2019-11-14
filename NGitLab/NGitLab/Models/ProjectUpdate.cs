using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public sealed class ProjectUpdate
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "path")]
        public string Path { get; set; }

        [DataMember(Name = "default_branch")]
        public string DefaultBranch { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "issues_enabled")]
        public bool? IssuesEnabled { get; set; }

        [DataMember(Name = "merge_requests_enabled")]
        public bool? MergeRequestsEnabled { get; set; }

        [DataMember(Name = "jobs_enabled")]
        public bool? JobsEnabled { get; set; }

        [DataMember(Name = "wiki_enabled")]
        public bool? WikiEnabled { get; set; }

        [DataMember(Name = "snippets_enabled")]
        public bool? SnippetsEnabled { get; set; }

        [DataMember(Name = "resolve_outdated_diff_discussions")]
        public bool? ResolveOutdatedDiffDiscussions { get; set; }

        [DataMember(Name = "container_registry_enabled")]
        public bool? ContainerRegistryEnabled { get; set; }

        [DataMember(Name = "shared_runners_enabled")]
        public bool? SharedRunnersEnabled { get; set; }

        [DataMember(Name = "visibility")]
        public VisibilityLevel? Visibility { get; set; }

        [DataMember(Name = "public_builds")]
        public bool? PublicBuilds { get; set; }

        [DataMember(Name = "only_allow_merge_if_pipeline_succeeds")]
        public bool? OnlyAllowMergeIfPipelineSucceeds { get; set; }

        [DataMember(Name = "only_allow_merge_if_all_discussions_are_resolved")]
        public bool? OnlyAllowMergeIfAllDiscussionsAreResolved { get; set; }

        [DataMember(Name = "remove_source_branch_after_merge")]
        public bool? RemoveSourceBranchAfterMerge { get; set; }

        [DataMember(Name = "lfs_enabled")]
        public bool? LfsEnabled { get; set; }

        [DataMember(Name = "request_access_enabled")]
        public bool? RequestAccessEnabled { get; set; }

        [DataMember(Name = "packages_enabled")]
        public bool? PackagesEnabled { get; set; }
    }
}