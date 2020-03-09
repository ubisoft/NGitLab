using System.Collections.Generic;
using System.Linq;
using NGitLab.Models;

namespace NGitLab.Mock.Clients
{
    internal sealed class JobClient : ClientBase, IJobClient
    {
        private readonly int _projectId;

        public JobClient(ClientContext context, int projectId)
            : base(context)
        {
            _projectId = projectId;
        }

        public Models.Job Get(int jobId)
        {
            var project = GetProject(_projectId, ProjectPermission.View);
            var job = project.Jobs.GetById(jobId);

            if (job == null)
                throw new GitLabNotFoundException();

            return job.ToJobClient();
        }

        public byte[] GetJobArtifacts(int jobId)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Models.Job> GetJobs(JobScopeMask scope)
        {
            var project = GetProject(_projectId, ProjectPermission.View);

            if (scope == JobScopeMask.All)
                return project.Jobs.Select(j => j.ToJobClient());

            var jobs = project.Jobs.Where(j => string.Equals(j.Status.ToString(), scope.ToString(), System.StringComparison.OrdinalIgnoreCase));
            return jobs.Select(j => j.ToJobClient());
        }

        public string GetTrace(int jobId)
        {
            throw new System.NotImplementedException();
        }

        public Models.Job RunAction(int jobId, JobAction action)
        {
            var project = GetProject(_projectId, ProjectPermission.View);
            var job = project.Jobs.GetById(jobId);

            switch (action)
            {
                case JobAction.Cancel:
                    job.Status = JobStatus.Canceled;
                    break;
                case JobAction.Erase:
                    job.Artifacts = null;
                    break;
                case JobAction.Play:
                    job.Status = JobStatus.Running;
                    break;
                case JobAction.Retry:
                    var retryJob = job.Clone();
                    retryJob.Status = JobStatus.Running;
                    project.Jobs.Add(retryJob, project.Pipelines.GetById((int)job.Pipeline.Id));
                    return retryJob.ToJobClient();
            }

            return job.ToJobClient();
        }
    }
}
