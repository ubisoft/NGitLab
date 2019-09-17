using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Mock.Clients
{
    internal sealed class PipelineClient : ClientBase, IPipelineClient
    {
        public PipelineClient(ClientContext context, int projectId)
            : base(context)
        {
        }

        public Pipeline this[int id] => throw new System.NotImplementedException();

        public IEnumerable<PipelineBasic> All => throw new System.NotImplementedException();

        public IEnumerable<Job> AllJobs => throw new System.NotImplementedException();

        public Pipeline Create(string @ref)
        {
            throw new System.NotImplementedException();
        }

        public Pipeline CreatePipelineWithTrigger(string token, string @ref, Dictionary<string, string> variables)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(int pipelineId)
        {
            throw new System.NotImplementedException();
        }

        public Job[] GetJobs(int pipelineId)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Job> GetJobsInProject(JobScope scope)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<PipelineBasic> Search(PipelineQuery query)
        {
            throw new System.NotImplementedException();
        }
    }
}
