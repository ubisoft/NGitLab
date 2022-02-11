using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NGitLab.Models;

namespace NGitLab.Mock
{
    /// <summary>
    /// Starting point of your mock, instantiate GitLab client
    /// and add fake content to it.
    /// </summary>
    public sealed class GitLabServer : GitLabObject, IDisposable
    {
        // Setting to a 'magic' high value to avoid having equalities between IDs and IiDs
        private int _lastProjectId = 10000;
        private int _lastGroupId = 10000;
        private int _lastMergeRequestId = 10000;
        private int _lastRunnerId = 10000;
        private int _lastIssueId = 10000;
        private int _lastMilestoneId = 10000;
        private int _lastPipelineId = 10000;
        private int _lastJobId = 10000;
        private int _lastBadgeId = 10000;
        private int _lastLabelId = 10000;

        public GitLabServer()
        {
            Groups = new GroupCollection(this);
            Users = new UserCollection(this);
            SystemHooks = new SystemHookCollection(this);
            Events = new EventCollection(this);
        }

        public string DefaultBranchName { get; set; } = "main";

        public Uri Url { get; set; } = new Uri("https://gitlab.example.com/", UriKind.Absolute);

        public GitLabVersion Version { get; set; } = new GitLabVersion { Version = "1.0.0", Revision = "rev1" };

        public GroupCollection Groups { get; }

        public UserCollection Users { get; }

        public SystemHookCollection SystemHooks { get; }

        public EventCollection Events { get; }

        public VisibilityLevel DefaultForkVisibilityLevel { get; set; } = VisibilityLevel.Private;

        public IGraphQLClient DefaultGraphQLClient { get; set; }

        public IGitLabClient CreateClient(User user)
        {
            if (!Users.Contains(user))
            {
                Users.Add(user);
            }

            return new Clients.GitLabClient(new Clients.ClientContext(this, user));
        }

        public IEnumerable<Group> AllGroups
        {
            get
            {
                foreach (var group in Groups)
                {
                    yield return group;
                    foreach (var subGroup in group.DescendantGroups)
                    {
                        yield return subGroup;
                    }
                }
            }
        }

        public IEnumerable<Runner> AllRunners => AllProjects.SelectMany(p => p.RegisteredRunners);

        public IEnumerable<Project> AllProjects => AllGroups.SelectMany(group => group.Projects);

        public void Dispose()
        {
            foreach (var project in AllProjects)
            {
                project.Repository.Dispose();
            }
        }

        internal int GetNewGroupId() => Interlocked.Increment(ref _lastGroupId);

        internal int GetNewProjectId() => Interlocked.Increment(ref _lastProjectId);

        internal int GetNewMergeRequestId() => Interlocked.Increment(ref _lastMergeRequestId);

        internal int GetNewIssueId() => Interlocked.Increment(ref _lastIssueId);

        internal int GetNewMilestoneId() => Interlocked.Increment(ref _lastMilestoneId);

        internal int GetNewRunnerId() => Interlocked.Increment(ref _lastRunnerId);

        internal int GetNewPipelineId() => Interlocked.Increment(ref _lastPipelineId);

        internal int GetNewJobId() => Interlocked.Increment(ref _lastJobId);

        internal int GetNewBadgeId() => Interlocked.Increment(ref _lastBadgeId);

        internal int GetNewLabelId() => Interlocked.Increment(ref _lastLabelId);

        internal string MakeUrl(string relativeUrl)
        {
            return new Uri(Url, relativeUrl).AbsoluteUri;
        }
    }
}
