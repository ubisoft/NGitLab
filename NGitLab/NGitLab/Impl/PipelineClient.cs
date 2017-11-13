using System;
using System.Collections.Generic;
using System.Linq;
using NGitLab.Models;

namespace NGitLab.Impl
{
    public class PipelineClient : IPipelineClient
    {
        private readonly API _api;
        private readonly string _projectPath;
        private readonly string _pipelinesPath;

        public PipelineClient(API api, int projectId)
        {
            _api = api;
            _projectPath = $"{Project.Url}/{projectId}";
            _pipelinesPath = $"{Project.Url}/{projectId}/pipelines";
        }

        public IEnumerable<PipelineBasic> All => _api.Get().GetAll<PipelineBasic>($"{_pipelinesPath}");

        public IEnumerable<Job> AllJobs => _api.Get().GetAll<Job>($"{_projectPath}/jobs");

        public IEnumerable<Job> GetJobsInProject(JobScope scope)
        {
            string url = $"{_projectPath}/jobs";

            if (scope != JobScope.All)
            {
                url = Utils.AddParameter(url, "scope", scope.ToString().ToLowerInvariant());
            }

            return _api.Get().GetAll<Job>(url);
        }

        public Pipeline this[int id] => _api.Get().To<Pipeline>($"{_pipelinesPath}/{id}");

        public Job[] GetJobs(int pipelineId)
        {
            // For a reason gitlab returns the jobs in the reverse order as
            // they appear in their UI. Here we reverse them!

            var jobs = _api.Get().GetAll<Job>($"{_pipelinesPath}/{pipelineId}/jobs").Reverse().ToArray();
            return jobs;
        }
    }
}