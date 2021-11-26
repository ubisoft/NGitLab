using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab
{
    public interface IJobClient
    {
        IEnumerable<Job> GetJobs(JobScopeMask scope);

        IEnumerable<Job> GetJobs(JobQuery query);

        GitLabCollectionResponse<Job> GetJobsAsync(JobQuery query);

        Job RunAction(int jobId, JobAction action);

        Task<Job> RunActionAsync(int jobId, JobAction action, CancellationToken cancellationToken = default);

        Job Get(int jobId);

        Task<Job> GetAsync(int jobId, CancellationToken cancellationToken = default);

        byte[] GetJobArtifacts(int jobId);

        string GetTrace(int jobId);

        Task<string> GetTraceAsync(int jobId, CancellationToken cancellationToken = default);
    }
}
