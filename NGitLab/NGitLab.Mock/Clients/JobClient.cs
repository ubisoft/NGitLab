using System.Collections.Generic;
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

        public Job Get(int jobId)
        {
            throw new System.NotImplementedException();
        }

        public byte[] GetJobArtifacts(int jobId)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Job> GetJobs(JobScopeMask scope)
        {
            throw new System.NotImplementedException();
        }

        public string GetTrace(int jobId)
        {
            throw new System.NotImplementedException();
        }

        public Job RunAction(int jobId, JobAction action)
        {
            throw new System.NotImplementedException();
        }
    }
}
