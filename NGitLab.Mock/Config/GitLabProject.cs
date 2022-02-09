using NGitLab.Models;

namespace NGitLab.Mock.Config
{
    /// <summary>
    /// Describe a GitLab project
    /// </summary>
    public class GitLabProject : GitLabObject<GitLabConfig>
    {
        public GitLabProject()
        {
            Commits = new GitLabCommitsCollection(this);
            Issues = new GitLabIssuesCollection(this);
            MergeRequests = new GitLabMergeRequestsCollection(this);
            Permissions = new GitLabPermissionsCollection(this);
            Labels = new GitLabLabelsCollection(this);
            Milestones = new GitLabMilestonesCollection(this);
            Pipelines = new GitLabPipelinesCollection(this);
        }

        /// <summary>
        /// Name (required)
        /// </summary>
        public string Name { get; set; }

        public string Namespace { get; set; }

        public string DefaultBranch { get; set; }

        public string Description { get; set; }

        public RepositoryAccessLevel ForkingAccessLevel { get; set; }

        /// <summary>
        /// Path where to clone repository after server resolving
        /// </summary>
        public string ClonePath { get; set; }

        /// <summary>
        /// Parameters used when cloning the local repository (for instance you can specify `--filter=blob:none` to do a blobless clone)
        /// </summary>
        public string CloneParameters { get; set; }

        public VisibilityLevel? Visibility { get; set; }

        public GitLabCommitsCollection Commits { get; }

        public GitLabIssuesCollection Issues { get; }

        public GitLabMergeRequestsCollection MergeRequests { get; }

        public GitLabPermissionsCollection Permissions { get; }

        public GitLabLabelsCollection Labels { get; }

        public GitLabMilestonesCollection Milestones { get; }

        public GitLabPipelinesCollection Pipelines { get; }
    }

    public class GitLabProjectsCollection : GitLabCollection<GitLabProject, GitLabConfig>
    {
        internal GitLabProjectsCollection(GitLabConfig parent)
            : base(parent)
        {
        }
    }
}
