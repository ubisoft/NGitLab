using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab
{
    public interface IJobClient
    {
        IEnumerable<Job> GetJobs(JobScopeMask scope);
        Job RunAction(int jobId, JobAction action);
        Job Get(int jobId);
        string GetTrace(int jobId);
    }
}
