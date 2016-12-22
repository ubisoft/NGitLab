using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Impl
{
    public class PipelineClient : IPipelineClient
    {
        private readonly API _api;
        private readonly string _path;

        public PipelineClient(API api, int projectId)
        {
            _api = api;

            var projectPath = Project.Url + "/" + projectId;
            _path = projectPath + "/pipelines";
        }

        public IEnumerable<Pipeline> All => _api.Get().GetAll<Pipeline>(_path);

        public Pipeline this[int id] => _api.Get().To<Pipeline>($"{_path}/{id}");
    }
}