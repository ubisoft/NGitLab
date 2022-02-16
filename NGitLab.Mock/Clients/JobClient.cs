using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Mock.Internals;
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
            using (Context.BeginOperationScope())
            {
                var project = GetProject(_projectId, ProjectPermission.View);
                var job = project.Jobs.GetById(jobId);

                if (job == null)
                    throw new GitLabNotFoundException();

                return job.ToJobClient();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "MA0042:Do not use blocking calls in an async method", Justification = "Would be an infinite recursion")]
        public async Task<Models.Job> GetAsync(int jobId, CancellationToken cancellationToken = default)
        {
            await Task.Yield();
            return Get(jobId);
        }

        public byte[] GetJobArtifacts(int jobId)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Models.Job> GetJobs(JobScopeMask scope)
        {
            return GetJobs(new JobQuery { Scope = scope });
        }

        public IEnumerable<Models.Job> GetJobs(JobQuery query)
        {
            using (Context.BeginOperationScope())
            {
                var project = GetProject(_projectId, ProjectPermission.View);

                if (query.Scope == JobScopeMask.All)
                    return project.Jobs.Select(j => j.ToJobClient());

                var scopes = new List<string>();
                foreach (Enum value in Enum.GetValues(query.Scope.GetType()))
                {
                    if (query.Scope.HasFlag(value))
                    {
                        scopes.Add(value.ToString());
                    }
                }

                var jobs = project.Jobs.Where(j => scopes.Any(scope => string.Equals(j.Status.ToString(), scope, System.StringComparison.OrdinalIgnoreCase)));
                return jobs.Select(j => j.ToJobClient()).ToList();
            }
        }

        public GitLabCollectionResponse<Models.Job> GetJobsAsync(JobQuery query)
        {
            return GitLabCollectionResponse.Create(GetJobs(query));
        }

        public string GetTrace(int jobId)
        {
            using (Context.BeginOperationScope())
            {
                var project = GetProject(_projectId, ProjectPermission.View);
                var job = project.Jobs.GetById(jobId);

                if (job == null)
                    throw new GitLabNotFoundException();

                return job.Trace;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "MA0042:Do not use blocking calls in an async method", Justification = "Would be an infinite recursion")]
        public async Task<string> GetTraceAsync(int jobId, CancellationToken cancellationToken = default)
        {
            await Task.Yield();
            return GetTrace(jobId);
        }

        public Models.Job RunAction(int jobId, JobAction action)
        {
            using (Context.BeginOperationScope())
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
                        job.Trace = null;
                        break;
                    case JobAction.Play:
                        job.Status = JobStatus.Running;
                        break;
                    case JobAction.Retry:
                        var retryJob = job.Clone();
                        retryJob.Status = JobStatus.Running;
                        project.Jobs.Add(retryJob, project.Pipelines.GetById(job.Pipeline.Id));
                        return retryJob.ToJobClient();
                }

                return job.ToJobClient();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "MA0042:Do not use blocking calls in an async method", Justification = "Would be an infinite recursion")]
        public async Task<Models.Job> RunActionAsync(int jobId, JobAction action, CancellationToken cancellationToken = default)
        {
            await Task.Yield();
            return RunAction(jobId, action);
        }
    }
}
