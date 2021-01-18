using System;
using System.Linq;

namespace NGitLab.Mock.Clients
{
    internal sealed class CommitClient : ClientBase, ICommitClient
    {
        private readonly int _projectId;

        public CommitClient(ClientContext context, int projectId)
            : base(context)
        {
            _projectId = projectId;
        }

        public Models.Commit Create(Models.CommitCreate commit)
        {
            using (Context.BeginOperationScope())
            {
                var project = GetProject(commit.ProjectId, ProjectPermission.Contribute);
                var gitCommit = project.Repository.Commit(commit);

                return gitCommit.ToCommitClient(project.CommitInfos.SingleOrDefault(c => string.Equals(c.Sha, gitCommit.Sha, StringComparison.Ordinal)));
            }
        }

        public Models.Commit GetCommit(string @ref)
        {
            using (Context.BeginOperationScope())
            {
                var project = GetProject(_projectId, ProjectPermission.View);
                var commit = project.Repository.GetCommit(@ref);

                return commit?.ToCommitClient(project.CommitInfos.SingleOrDefault(c => string.Equals(c.Sha, commit.Sha, StringComparison.Ordinal)));
            }
        }

        public JobStatus GetJobStatus(string branchName)
        {
            throw new NotImplementedException();
        }
    }
}
