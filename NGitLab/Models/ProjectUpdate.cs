using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public sealed class ProjectUpdate
    {
        [DataMember(Name = "name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [DataMember(Name = "path")]
        [JsonPropertyName("path")]
        public string Path { get; set; }

        [DataMember(Name = "default_branch")]
        [JsonPropertyName("default_branch")]
        public string DefaultBranch { get; set; }

        [DataMember(Name = "description")]
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [DataMember(Name = "issues_enabled")]
        [JsonPropertyName("issues_enabled")]
        [Obsolete("Deprecated by GitLab. Use IssuesAccessLevel instead")]
        public bool? IssuesEnabled { get; set; }

        [DataMember(Name = "issues_access_level")]
        [JsonPropertyName("issues_access_level")]
        public string IssuesAccessLeve { get; set; }

        [DataMember(Name = "merge_requests_enabled")]
        [JsonPropertyName("merge_requests_enabled")]
        [Obsolete("Deprecated by GitLab. Use MergeRequestsAccessLevel instead")]
        public bool? MergeRequestsEnabled { get; set; }

        [DataMember(Name = "merge_requests_access_level")]
        [JsonPropertyName("merge_requests_access_level")]
        public string MergeRequestsAccessLevel { get; set; }

        [DataMember(Name = "jobs_enabled")]
        [JsonPropertyName("jobs_enabled")]
        [Obsolete("Deprecated by GitLab. Use BuildsAccessLevel instead")]
        public bool? JobsEnabled { get; set; }

        [DataMember(Name = "builds_access_level")]
        [JsonPropertyName("builds_access_level")]
        public string BuildsAccessLevel { get; set; }

        [DataMember(Name = "wiki_enabled")]
        [JsonPropertyName("wiki_enabled")]
        [Obsolete("Deprecated by GitLab. Use WikiAccessLevel instead")]
        public bool? WikiEnabled { get; set; }

        [DataMember(Name = "wiki_access_level")]
        [JsonPropertyName("wiki_access_level")]
        public string WikiAccessLevel { get; set; }

        [DataMember(Name = "snippets_enabled")]
        [JsonPropertyName("snippets_enabled")]
        [Obsolete("Deprecated by GitLab. Use SnippetsAccessLevel instead")]
        public bool? SnippetsEnabled { get; set; }

        [DataMember(Name = "snippets_access_level")]
        [JsonPropertyName("snippets_access_level")]
        public string SnippetsAccessLevel { get; set; }

        [DataMember(Name = "resolve_outdated_diff_discussions")]
        [JsonPropertyName("resolve_outdated_diff_discussions")]
        public bool? ResolveOutdatedDiffDiscussions { get; set; }

        [DataMember(Name = "container_registry_enabled")]
        [JsonPropertyName("container_registry_enabled")]
        public bool? ContainerRegistryEnabled { get; set; }

        [DataMember(Name = "shared_runners_enabled")]
        [JsonPropertyName("shared_runners_enabled")]
        public bool? SharedRunnersEnabled { get; set; }

        [DataMember(Name = "visibility")]
        [JsonPropertyName("visibility")]
        public VisibilityLevel? Visibility { get; set; }

        [DataMember(Name = "public_builds")]
        [JsonPropertyName("public_builds")]
        public bool? PublicBuilds { get; set; }

        [DataMember(Name = "only_allow_merge_if_pipeline_succeeds")]
        [JsonPropertyName("only_allow_merge_if_pipeline_succeeds")]
        public bool? OnlyAllowMergeIfPipelineSucceeds { get; set; }

        [DataMember(Name = "only_allow_merge_if_all_discussions_are_resolved")]
        [JsonPropertyName("only_allow_merge_if_all_discussions_are_resolved")]
        public bool? OnlyAllowMergeIfAllDiscussionsAreResolved { get; set; }

        [DataMember(Name = "remove_source_branch_after_merge")]
        [JsonPropertyName("remove_source_branch_after_merge")]
        public bool? RemoveSourceBranchAfterMerge { get; set; }

        [DataMember(Name = "lfs_enabled")]
        [JsonPropertyName("lfs_enabled")]
        public bool? LfsEnabled { get; set; }

        [DataMember(Name = "request_access_enabled")]
        [JsonPropertyName("request_access_enabled")]
        public bool? RequestAccessEnabled { get; set; }

        [DataMember(Name = "packages_enabled")]
        [JsonPropertyName("packages_enabled")]
        public bool? PackagesEnabled { get; set; }

        [DataMember(Name = "build_timeout")]
        [JsonPropertyName("build_timeout")]
        public int? BuildTimeout;

        [DataMember(Name = "tag_list")]
        [JsonPropertyName("tag_list")]
        public string[] TagList;
    }
}
