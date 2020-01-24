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
            var project = GetProject(commit.ProjectId, ProjectPermission.Contribute);
            var gitCommit = project.Repository.Commit(commit);

            return gitCommit.ToCommitClient(Server.CommitInfos.SingleOrDefault(c => c.Sha == gitCommit.Sha));
        }

        public Models.Commit GetCommit(string @ref)
        {
            var project = GetProject(_projectId, ProjectPermission.View);
            var commit = project.Repository.GetCommit(@ref);

            return commit?.ToCommitClient(Server.CommitInfos.SingleOrDefault(c => c.Sha == commit.Sha));
        }

        public JobStatus GetJobStatus(string branchName)
        {
            throw new NotImplementedException();
        }
    }
}
