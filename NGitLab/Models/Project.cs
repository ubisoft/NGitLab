using System;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

[DebuggerDisplay("{" + nameof(PathWithNamespace) + "}")]
public class Project
{
    public const string Url = "/projects";

    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("avatar_url")]
    public string AvatarUrl { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("name_with_namespace")]
    public string NameWithNamespace { get; set; }

    [JsonPropertyName("open_issues_count")]
    public int OpenIssuesCount { get; set; }

    [JsonPropertyName("jobs_enabled")]
    [Obsolete("Deprecated by GitLab. Use BuildsAccessLevel instead")]
    public bool JobEnabled { get; set; }

    [JsonPropertyName("builds_access_level")]
    public RepositoryAccessLevel BuildsAccessLevel { get; set; }

    [JsonPropertyName("snippets_enabled")]
    [Obsolete("Deprecated by GitLab. Use SnippetsAccessLevel instead")]
    public bool SnippetsEnabled { get; set; }

    [JsonPropertyName("snippets_access_level")]
    public RepositoryAccessLevel SnippetsAccessLevel { get; set; }

    [JsonPropertyName("resolve_outdated_diff_discussions")]
    public bool ResolveOutdatedDiffDiscussions { get; set; }

    [JsonPropertyName("container_registry_enabled")]
    public bool ContainerRegistryEnabled { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("default_branch")]
    public string DefaultBranch { get; set; }

    [JsonPropertyName("owner")]
    public User Owner { get; set; }

    [JsonPropertyName("public")]
    public bool Public { get; set; }

    [JsonPropertyName("path")]
    public string Path { get; set; }

    [JsonPropertyName("path_with_namespace")]
    public string PathWithNamespace { get; set; }

    [JsonPropertyName("issues_enabled")]
    [Obsolete("Deprecated by GitLab. Use IssuesAccessLevel instead")]
    public bool IssuesEnabled { get; set; }

    [JsonPropertyName("issues_access_level")]
    public RepositoryAccessLevel IssuesAccessLevel { get; set; }

    [JsonPropertyName("merge_pipelines_enabled")]
    public bool MergePipelinesEnabled { get; set; }

    [JsonPropertyName("merge_requests_enabled")]
    [Obsolete("Deprecated by GitLab. Use MergeRequestsAccessLevel instead")]
    public bool MergeRequestsEnabled { get; set; }

    [JsonPropertyName("merge_requests_access_level")]
    public RepositoryAccessLevel MergeRequestsAccessLevel { get; set; }

    [JsonPropertyName("merge_trains_enabled")]
    public bool MergeTrainsEnabled { get; set; }

    [JsonPropertyName("repository_access_level")]
    public RepositoryAccessLevel RepositoryAccessLevel { get; set; }

    [JsonPropertyName("merge_method")]
    public string MergeMethod { get; set; }

    [JsonPropertyName("wall_enabled")]
    public bool WallEnabled { get; set; }

    [JsonPropertyName("wiki_enabled")]
    [Obsolete("Deprecated by GitLab. Use WikiAccessLevel instead")]
    public bool WikiEnabled { get; set; }

    [JsonPropertyName("wiki_access_level")]
    public RepositoryAccessLevel WikiAccessLevel { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("last_activity_at")]
    public DateTime LastActivityAt { get; set; }

    [JsonPropertyName("creator_id")]
    public long CreatorId { get; set; }

    [JsonPropertyName("ssh_url_to_repo")]
    public string SshUrl { get; set; }

    [JsonPropertyName("http_url_to_repo")]
    public string HttpUrl { get; set; }

    [JsonPropertyName("web_url")]
    public string WebUrl { get; set; }

    [JsonPropertyName("namespace")]
    public Namespace Namespace { get; set; }

    [JsonPropertyName("import_status")]
    public string ImportStatus { get; set; }

    [JsonPropertyName("import_error")]
    public string ImportError { get; set; }

    [JsonPropertyName("archived")]
    public bool Archived { get; set; }

    [JsonPropertyName("shared_runners_enabled")]
    public bool SharedRunnersEnabled { get; set; }

    [JsonPropertyName("group_runners_enabled")]
    public bool GroupRunnersEnabled { get; set; }

    [JsonPropertyName("tag_list")]
    [Obsolete("Deprecated by GitLab. Use Topics instead")]
    public string[] TagList { get; set; }

    [JsonPropertyName("topics")]
    public string[] Topics { get; set; }

    [JsonPropertyName("star_count")]
    public int StarCount { get; set; }

    [JsonPropertyName("forks_count")]
    public int ForksCount { get; set; }

    [JsonPropertyName("forking_access_level")]
    public RepositoryAccessLevel ForkingAccessLevel { get; set; }

    [JsonPropertyName("runners_token")]
    public string RunnersToken { get; set; }

    [JsonPropertyName("public_jobs")]
    public bool PublicJobs { get; set; }

    [JsonPropertyName("forked_from_project")]
    public Project ForkedFromProject { get; set; }

    [JsonPropertyName("repository_storage")]
    public string RepositoryStorage { get; set; }

    [JsonPropertyName("only_allow_merge_if_pipeline_succeeds")]
    public bool OnlyAllowMergeIfPipelineSucceeds { get; set; }

    [JsonPropertyName("only_allow_merge_if_all_discussions_are_resolved")]
    public bool OnlyAllowMergeIfDiscussionsAreResolved { get; set; }

    [JsonPropertyName("printing_merge_requests_link_enabled")]
    public bool PrintingMergeRequestsLinkEnabled { get; set; }

    [JsonPropertyName("request_access_enabled")]
    public bool RequestAccessEnabled { get; set; }

    [JsonPropertyName("approvals_before_merge")]
    public int ApprovalsBeforeMerge { get; set; }

    [JsonPropertyName("statistics")]
    public ProjectStatistics Statistics { get; set; }

    [JsonPropertyName("_links")]
    public ProjectLinks Links { get; set; }

    [JsonPropertyName("visibility")]
    public VisibilityLevel VisibilityLevel { get; set; }

    /// <summary>
    /// The maximum amount of time, in seconds, that a job can run.
    /// </summary>
    [JsonPropertyName("build_timeout")]
    public int? BuildTimeout { get; set; }

    [JsonPropertyName("lfs_enabled")]
    public bool LfsEnabled { get; set; }

    [JsonPropertyName("empty_repo")]
    public bool EmptyRepo { get; set; }

    [JsonPropertyName("mirror")]
    public bool Mirror { get; set; }

    [JsonPropertyName("mirror_user_id")]
    public long MirrorUserId { get; set; }

    [JsonPropertyName("mirror_trigger_builds")]
    public bool MirrorTriggerBuilds { get; set; }

    [JsonPropertyName("only_mirror_protected_branches")]
    public bool OnlyMirrorProtectedBranch { get; set; }

    [JsonPropertyName("mirror_overwrites_diverged_branches")]
    public bool MirrorOverwritesDivergedBranches { get; set; }

    [JsonPropertyName("squash_option")]
    public DynamicEnum<SquashOption> SquashOption { get; set; }

    [JsonPropertyName("permissions")]
    public ProjectPermissions Permissions { get; set; }

    [JsonPropertyName("releases_access_level")]
    public RepositoryAccessLevel ReleasesAccessLevel { get; set; }

    [JsonPropertyName("requirements_access_level")]
    public RepositoryAccessLevel RequirementsAccessLevel { get; set; }

    [JsonPropertyName("mr_default_target_self")]
    public bool? MrDefaultTargetSelf { get; set; }

    [JsonPropertyName("ci_default_git_depth")]
    public int? CiDefaultGitDepth { get; set; }

    [JsonPropertyName("readme_url")]
    public string ReadmeUrl { get; set; }

    [JsonPropertyName("container_registry_image_prefix")]
    public string ContainerRegistryImagePrefix { get; set; }

    [JsonPropertyName("marked_for_deletion_at")]
    public DateTime? MarkedForDeletionAt { get; set; }

    [JsonPropertyName("marked_for_deletion_on")]
    public DateTime? MarkedForDeletionOn { get; set; }

    [JsonPropertyName("packages_enabled")]
    public bool PackagesEnabled { get; set; }

    [JsonPropertyName("container_expiration_policy")]
    public ContainerExpirationPolicy ContainerExpirationPolicy { get; set; }

    [JsonPropertyName("repository_object_format")]
    public string RepositoryObjectFormat { get; set; }

    [JsonPropertyName("service_desk_enabled")]
    public bool ServiceDeskEnabled { get; set; }

    [JsonPropertyName("service_desk_address")]
    public string ServiceDeskAddress { get; set; }

    [JsonPropertyName("can_create_merge_request_in")]
    public bool CanCreateMergeRequestIn { get; set; }

    [JsonPropertyName("pages_access_level")]
    public RepositoryAccessLevel PagesAccessLevel { get; set; }

    [JsonPropertyName("analytics_access_level")]
    public RepositoryAccessLevel AnalyticsAccessLevel { get; set; }

    [JsonPropertyName("container_registry_access_level")]
    public RepositoryAccessLevel ContainerRegistryAccessLevel { get; set; }

    [JsonPropertyName("security_and_compliance_access_level")]
    public RepositoryAccessLevel SecurityAndComplianceAccessLevel { get; set; }

    [JsonPropertyName("environments_access_level")]
    public RepositoryAccessLevel EnvironmentsAccessLevel { get; set; }

    [JsonPropertyName("feature_flags_access_level")]
    public RepositoryAccessLevel FeatureFlagsAccessLevel { get; set; }

    [JsonPropertyName("infrastructure_access_level")]
    public RepositoryAccessLevel InfrastructureAccessLevel { get; set; }

    [JsonPropertyName("monitor_access_level")]
    public RepositoryAccessLevel MonitorAccessLevel { get; set; }

    [JsonPropertyName("model_experiments_access_level")]
    public RepositoryAccessLevel ModelExperimentsAccessLevel { get; set; }

    [JsonPropertyName("model_registry_access_level")]
    public RepositoryAccessLevel ModelRegistryAccessLevel { get; set; }

    [JsonPropertyName("emails_disabled")]
    public bool? EmailsDisabled { get; set; }

    [JsonPropertyName("emails_enabled")]
    public bool? EmailsEnabled { get; set; }

    [JsonPropertyName("import_url")]
    public string ImportUrl { get; set; }

    [JsonPropertyName("import_type")]
    public string ImportType { get; set; }

    [JsonPropertyName("description_html")]
    public string DescriptionHtml { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [JsonPropertyName("ci_delete_pipelines_in_seconds")]
    public long? CiDeletePipelinesInSeconds { get; set; }

    [JsonPropertyName("ci_forward_deployment_enabled")]
    public bool CiForwardDeploymentEnabled { get; set; }

    [JsonPropertyName("ci_forward_deployment_rollback_allowed")]
    public bool CiForwardDeploymentRollbackAllowed { get; set; }

    [JsonPropertyName("ci_job_token_scope_enabled")]
    public bool CiJobTokenScopeEnabled { get; set; }

    [JsonPropertyName("ci_separated_caches")]
    public bool CiSeparatedCaches { get; set; }

    [JsonPropertyName("ci_allow_fork_pipelines_to_run_in_parent_project")]
    public bool CiAllowForkPipelinesToRunInParentProject { get; set; }

    [JsonPropertyName("ci_id_token_sub_claim_components")]
    public string[] CiIdTokenSubClaimComponents { get; set; }

    [JsonPropertyName("build_git_strategy")]
    public string BuildGitStrategy { get; set; }

    [JsonPropertyName("keep_latest_artifact")]
    public bool KeepLatestArtifact { get; set; }

    [JsonPropertyName("restrict_user_defined_variables")]
    public bool RestrictUserDefinedVariables { get; set; }

    [JsonPropertyName("ci_pipeline_variables_minimum_override_role")]
    public string CiPipelineVariablesMinimumOverrideRole { get; set; }

    [JsonPropertyName("runner_token_expiration_interval")]
    public long? RunnerTokenExpirationInterval { get; set; }

    [JsonPropertyName("auto_cancel_pending_pipelines")]
    public string AutoCancelPendingPipelines { get; set; }

    [JsonPropertyName("auto_devops_enabled")]
    public bool AutoDevopsEnabled { get; set; }

    [JsonPropertyName("auto_devops_deploy_strategy")]
    public string AutoDevopsDeployStrategy { get; set; }

    [JsonPropertyName("ci_push_repository_for_job_token_allowed")]
    public bool CiPushRepositoryForJobTokenAllowed { get; set; }

    [JsonPropertyName("ci_config_path")]
    public string CiConfigPath { get; set; }

    [JsonPropertyName("allow_merge_on_skipped_pipeline")]
    public bool? AllowMergeOnSkippedPipeline { get; set; }

    [JsonPropertyName("remove_source_branch_after_merge")]
    public bool RemoveSourceBranchAfterMerge { get; set; }

    [JsonPropertyName("merge_request_title_regex")]
    public string MergeRequestTitleRegex { get; set; }

    [JsonPropertyName("enforce_auth_checks_on_uploads")]
    public bool EnforceAuthChecksOnUploads { get; set; }

    [JsonPropertyName("suggestion_commit_message")]
    public string SuggestionCommitMessage { get; set; }

    [JsonPropertyName("merge_commit_template")]
    public string MergeCommitTemplate { get; set; }

    [JsonPropertyName("squash_commit_template")]
    public string SquashCommitTemplate { get; set; }

    [JsonPropertyName("issue_branch_template")]
    public string IssueBranchTemplate { get; set; }

    [JsonPropertyName("warn_about_potentially_unwanted_characters")]
    public bool WarnAboutPotentiallyUnwantedCharacters { get; set; }

    [JsonPropertyName("autoclose_referenced_issues")]
    public bool AutocloseReferencedIssues { get; set; }

    [JsonPropertyName("max_artifacts_size")]
    public long? MaxArtifactsSize { get; set; }
}
