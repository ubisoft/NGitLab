using System.Collections.Generic;
using System.Linq;
using NGitLab.Models;

namespace NGitLab.Impl
{
    public class PipelineClient : IPipelineClient
    {
        private readonly API _api;
        private readonly string _pipelinesPath;

        public PipelineClient(API api, int projectId)
        {
            _api = api;
            _pipelinesPath = $"{Project.Url}/{projectId}/pipelines";
        }

        public IEnumerable<Pipeline> All => _api.Get().GetAll<Pipeline>($"{_pipelinesPath}");

        public Pipeline this[int id] => _api.Get().To<Pipeline>($"{_pipelinesPath}/{id}");

        public Job[] GetJobs(int pipelineId)
        {
            return _api.Get().GetAll<Job>($"{_pipelinesPath}/{pipelineId}/jobs").ToArray();
        }
    }
}