using System;
using System.Collections.Generic;

namespace NGitLab.Mock.Config
{
    /// <summary>
    /// Describe an issue in a GitLab project
    /// </summary>
    public class GitLabIssue : GitLabObject<GitLabProject>
    {
        /// <summary>
        /// Title (required)
        /// </summary>
        public string Title { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Labels names
        /// </summary>
        public IList<string> Labels { get; } = new List<string>();

        /// <summary>
        /// Author username (required)
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Assignee username
        /// </summary>
        public string Assignee { get; set; }

        /// <summary>
        /// Milestone title
        /// </summary>
        public string Milestone { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public DateTime? ClosedAt { get; set; }
    }

    public class GitLabIssuesCollection : GitLabCollection<GitLabIssue, GitLabProject>
    {
        internal GitLabIssuesCollection(GitLabProject parent)
            : base(parent)
        {
        }
    }
}
