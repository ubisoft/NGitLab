using System;

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
            return project.Repository.Commit(Context.User, commit).ToCommitClient();
        }

        public Models.Commit GetCommit(string @ref)
        {
            var project = GetProject(_projectId, ProjectPermission.View);
            return project.Repository.GetCommit(@ref).ToCommitClient();
        }

        public JobStatus GetJobStatus(string branchName)
        {
            throw new NotImplementedException();
        }
    }
}
