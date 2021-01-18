using System.Collections.Generic;
using System.Linq;
using NGitLab.Models;

namespace NGitLab.Mock.Clients
{
    internal sealed class CommitStatusClient : ClientBase, ICommitStatusClient
    {
        private readonly int _projectId;

        public CommitStatusClient(ClientContext context, int projectId)
            : base(context)
        {
            _projectId = projectId;
        }

        public CommitStatusCreate AddOrUpdate(CommitStatusCreate status)
        {
            using (Context.BeginOperationScope())
            {
                var project = GetProject(_projectId, ProjectPermission.Contribute);
                var commitStatus = project.CommitStatuses.FirstOrDefault(cs => Equals(cs, status));
                if (commitStatus == null)
                {
                    commitStatus = new CommitStatus();
                    project.CommitStatuses.Add(commitStatus);
                }

                commitStatus.Name = status.Name;
                commitStatus.Description = status.Description;
                commitStatus.Coverage = status.Coverage;
                commitStatus.Ref = status.Ref;
                commitStatus.Sha = status.CommitSha;
                commitStatus.Status = status.Status;
                commitStatus.TargetUrl = status.TargetUrl;
                return commitStatus.ToClientCommitStatusCreate();
            }

            static bool Equals(CommitStatus a, CommitStatusCreate b)
            {
                return string.Equals(a.Name, b.Name, System.StringComparison.Ordinal) &&
                       string.Equals(a.Ref, b.Ref, System.StringComparison.Ordinal) &&
                       string.Equals(a.Sha, b.CommitSha, System.StringComparison.OrdinalIgnoreCase) &&
                       string.Equals(a.TargetUrl, b.TargetUrl, System.StringComparison.Ordinal);
            }
        }

        public IEnumerable<Models.CommitStatus> AllBySha(string commitSha)
        {
            using (Context.BeginOperationScope())
            {
                var project = GetProject(_projectId, ProjectPermission.View);
                return project.CommitStatuses
                    .Where(p => string.Equals(p.Sha, commitSha, System.StringComparison.OrdinalIgnoreCase))
                    .Select(p => p.ToClientCommitStatus())
                    .ToList();
            }
        }
    }
}
