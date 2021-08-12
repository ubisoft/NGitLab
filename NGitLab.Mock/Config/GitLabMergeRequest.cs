﻿using System;
using System.Collections.Generic;

namespace NGitLab.Mock.Config
{
    /// <summary>
    /// Describe a merge request in a GitLab project
    /// </summary>
    public class GitLabMergeRequest : GitLabObject<GitLabProject>
    {
        /// <summary>
        /// Title (required)
        /// </summary>
        public string Title { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Author username (required if default user not defined)
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Assignee username
        /// </summary>
        public string Assignee { get; set; }

        public string SourceBranch { get; set; }

        public string TargetBranch { get; set; } = "main";

        /// <summary>
        /// Labels names
        /// </summary>
        public IList<string> Labels { get; } = new List<string>();

        /// <summary>
        /// Approvers usernames
        /// </summary>
        public IList<string> Approvers { get; } = new List<string>();

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public DateTime? MergedAt { get; set; }

        public DateTime? ClosedAt { get; set; }
    }

    public class GitLabMergeRequestsCollection : GitLabCollection<GitLabMergeRequest, GitLabProject>
    {
        internal GitLabMergeRequestsCollection(GitLabProject parent)
            : base(parent)
        {
        }
    }
}
