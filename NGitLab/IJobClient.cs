using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab;

public interface IJobClient
{
    IEnumerable<Job> GetJobs(JobScopeMask scope);

    IEnumerable<Job> GetJobs(JobQuery query);

    GitLabCollectionResponse<Job> GetJobsAsync(JobQuery query);

    Job RunAction(long jobId, JobAction action);

    Task<Job> RunActionAsync(long jobId, JobAction action, CancellationToken cancellationToken = default);

    Job Get(long jobId);

    Task<Job> GetAsync(long jobId, CancellationToken cancellationToken = default);

    byte[] GetJobArtifacts(long jobId);

    byte[] GetJobArtifact(long jobId, string path);

    byte[] GetJobArtifact(JobArtifactQuery query);

    string GetTrace(long jobId);

    Task<string> GetTraceAsync(long jobId, CancellationToken cancellationToken = default);
}
