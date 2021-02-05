using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab
{
    public interface IJobClient
    {
        IEnumerable<Job> GetJobs(JobScopeMask scope);

        IEnumerable<Job> GetJobs(JobQuery query);

        Job RunAction(int jobId, JobAction action);

        Job Get(int jobId);

        byte[] GetJobArtifacts(int jobId);

        string GetTrace(int jobId);
    }
}
