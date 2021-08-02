using System;
using System.Collections.Generic;

namespace NGitLab.Mock.Fluent
{
    public class GitLabIssue : GitLabObject<GitLabProject>
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public IList<string> Labels { get; } = new List<string>();

        public string Author { get; set; }

        public string Assignee { get; set; }

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
