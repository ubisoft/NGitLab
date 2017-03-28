using System.Collections.Generic;
using System.Linq;
using NGitLab.Models;

namespace NGitLab.Impl
{
    public class PipelineClient : IPipelineClient
    {
        private readonly API _api;
        private readonly string _projectPath;

        public PipelineClient(API api, int projectId)
        {
            _api = api;
            _projectPath = Project.Url + "/" + projectId;
        }

        public IEnumerable<Pipeline> All => _api.Get().GetAll<Pipeline>($"{_projectPath}/pipelines");

        public Pipeline this[int id] => _api.Get().To<Pipeline>($"{_projectPath}/pipelines/{id}");

        public Job[] GetJobs(int pipelineId)
        {
            return _api.Get().GetAll<Job>($"{_projectPath}/pipeline/{pipelineId}/jobs").ToArray();
        }
    }
}